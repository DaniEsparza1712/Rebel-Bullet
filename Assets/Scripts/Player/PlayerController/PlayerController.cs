using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class StateVars
{
    public Vector3 moveVector;
    public bool aiming;
    public bool shooting;
    public bool gunOut = false;
    public bool airborne = false;
    public int jumpCount = 0;
    public StateBase prevState;
}

[RequireComponent(typeof(CapsuleCollider))]
[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(RagdollManager))]
public class PlayerController : StatesMachine
{
    [Header("Movement Settings")] 
    [SerializeField] private float moveSpeed;
    [SerializeField] private float topSpeed;
    
    [Header("Jump Data")]
    [SerializeField] private float airMoveSpeed;
    [SerializeField] private float jumpForce;
    [SerializeField] private float jumpDeceleration;
    [SerializeField] private float jumpPeakDeceleration;
    [SerializeField] private float terminalVelocity;
    [SerializeField] private float fallDeceleration;
    [SerializeField] private float dashMultiplier;
    [SerializeField] private LayerMask floorMask;
    [SerializeField] private float coyoteTime;
    
    [Header("Dash Data")]
    [SerializeField] private float dashSpeed;
    [SerializeField] private float dashTime;
    [SerializeField] private bool dashFloat;

    [Header("Animation Settings")] [SerializeField]
    private float timeToSaveGun;
    
    private Rigidbody _rb;
    private CapsuleCollider _capsuleCollider;
    private CharacterController _characterController;
    private StateVars _playerVars;
    private EnemyHittable _hittable;
    private RagdollManager _ragdollManager;
    
    private PlayerInput _playerInput;
    private InputAction _moveAction;
    private InputAction _aimAction;
    private InputAction _jumpAction;
    private InputAction _dashAction;
    private InputAction _shootAction;
    private InputAction _detectiveAction;
    private InputAction _shootSpringAction;
    private InputAction _interact;
    
    private UIInput _uiInput;
    private InputAction _pauseAction;
    
    private GroundState _groundState;
    private Dash _dash;
    private Jump _jumpState;
    private JumpEnded _jumpEndedState;
    private Fall _fallState;
    private PlayerRagdoll _ragdollState;
    private GetUp _getUp;

    public EventHandler OnAimStarted;
    public EventHandler OnAimEnded;
    public EventHandler OnShootStarted;
    public EventHandler OnShootEnded;
    public EventHandler OnGunOut;
    public EventHandler OnGunIn;
    public EventHandler OnDeath;
    public EventHandler OnSpringShoot;

    public EventHandler OnDashTrailStart;
    public EventHandler OnDashTrailEnd;
    private EventHandler OnDashEnded;
    public EventHandler OnFall;
    public EventHandler OnLanded;
    public EventHandler OnInteract;
    public EventHandler OnPause;
    public EventHandler OnUnpause;
    
    private bool _checkingForGround = false;
    private bool _canAim = true;
    private bool _canShoot = true;
    private bool _detectiveActive = false;
    private bool _paused;

    private void Awake()
    {
        base.Awake();
        
        Cursor.lockState = CursorLockMode.Locked;
        
        _rb = GetComponent<Rigidbody>();
        _capsuleCollider = GetComponent<CapsuleCollider>();
        _characterController = GetComponent<CharacterController>();
        _hittable = GetComponent<EnemyHittable>();
        _ragdollManager = GetComponent<RagdollManager>();
        _playerVars = new StateVars();
        _playerInput = new PlayerInput();
        _uiInput = new UIInput();
        
        _groundState = new GroundState(_animator, moveSpeed, _playerVars, _characterController);
        _dash = new Dash(_animator, dashSpeed, _playerVars, _characterController, _rb,_capsuleCollider);
        _jumpState = new Jump(_animator, airMoveSpeed, dashMultiplier, jumpForce, jumpDeceleration, _playerVars, _characterController);
        _jumpEndedState = new JumpEnded(_animator, airMoveSpeed, jumpPeakDeceleration,_playerVars, _rb, _capsuleCollider, _characterController);
        _fallState = new Fall(_animator, airMoveSpeed, fallDeceleration, terminalVelocity, _playerVars, _characterController);
        _ragdollState = new PlayerRagdoll(_animator, _ragdollManager);
        _getUp = new GetUp(_animator, _ragdollManager, _characterController);

        OnAimStarted += (sender, args) =>
        {
            if (_canAim)
            {
                _playerVars.aiming = true;
                if(!_playerVars.gunOut)
                    OnGunOut?.Invoke(sender, args);
            }
        };

        OnAimEnded += (sender, args) =>
        {
            _playerVars.aiming = false;
            if (!_playerVars.shooting)
            {
                OnGunIn?.Invoke(sender, args);
            }
        };

        OnShootStarted += (sender, args) =>
        {
            StopCoroutine("WaitToGunIn");
            if (_canShoot)
            {
                _playerVars.shooting = true;
                if(!_playerVars.gunOut)
                    OnGunOut?.Invoke(sender, args);
            }
        };

        OnShootEnded += (sender, args) =>
        {
            _playerVars.shooting = false;
            StartCoroutine("WaitToGunIn");
        };
        
        OnGunOut += (sender, args) =>
        {
            _playerVars.gunOut = true;
            _animator.SetBool("Aim", true);
        };

        OnGunIn += (sender, args) =>
        {
            _playerVars.gunOut = false;
            _animator.SetBool("Aim", false);
        };
        
        ProcessStateChanges();
        
        _initialState = _groundState;
        _playerVars.prevState = _initialState;
        _currentState = _groundState;
    }

    private void Update()
    {
        base.Update();
        if (OnGround())
        {
            if (_checkingForGround)
            {
                if (_currentState == _dash)
                    return;
                ChangeState(_groundState);
                _playerVars.airborne = false;
                _checkingForGround = false;
            }
        }
        else if(!OnGround())
        {
            if (_currentState == _groundState && !_checkingForGround)
                StartCoroutine(WaitCoyoteTime());
        }
    }

    private void OnEnable()
    {
        _moveAction = _playerInput.Controller.Movement;
        _moveAction.Enable();
        _moveAction.performed += MoveInput;
        
        _aimAction = _playerInput.Controller.Aim;
        _aimAction.Enable();
        _aimAction.started += AimStarted;
        _aimAction.canceled += AimEnded;

        _jumpAction = _playerInput.Controller.Jump;
        _jumpAction.Enable();
        _jumpAction.started += JumpStarted;
        _jumpAction.canceled += JumpEnded;

        _dashAction = _playerInput.Controller.Dash;
        _dashAction.Enable();
        _dashAction.started += DashStarted;
        _dashAction.canceled += DashEnded;

        _shootAction = _playerInput.Controller.Shoot;
        _shootAction.Enable();
        _shootAction.started += ShootStarted;
        _shootAction.canceled += ShootEnded;

        _detectiveAction = _playerInput.Controller.DetectiveMode;
        _detectiveAction.Enable();
        _detectiveAction.performed += DetectiveInput;

        _shootSpringAction = _playerInput.Controller.SpringShoot;
        _shootSpringAction.Enable();
        _shootSpringAction.performed += SpringShoot;

        _interact = _playerInput.Controller.Interact;
        _interact.Enable();
        _interact.performed += InteractInput;

        _pauseAction = _uiInput.UI.Pause;
        _pauseAction.Enable();
        _pauseAction.performed += PauseInput;
    }

    private void ProcessStateChanges()
    {
        _ragdollState.OnHipsSpeedZero += (sender, args) =>
        {
            ChangeState(_getUp);
            StartCoroutine(WaitForState(_groundState, 0.25f));
        };
        _jumpState.OnReachedMinSpeed += (sender, args) =>
        {
            ChangeState(_jumpEndedState);
        };
        _jumpEndedState.OnReachedZero += (sender, args) =>
        {
            ChangeState(_fallState);
        };

        _hittable.OnHit += (sender, args) =>
        {
            var attack = args.attack;
            switch (attack.element)
            {
                case AttackElement.Concussive:
                    _animator.CrossFadeInFixedTime("FE_Death", 0.01f);
                    StartCoroutine(RagdollStateCoroutine(args));
                    break;
                default:
                    _animator.CrossFadeInFixedTime("GetHit", 0.2f);
                    _animator.CrossFadeInFixedTime("FE_Sorrow", 0.1f);
                    break;
            }
        };
        _hittable.OnDeath += (sender, args) =>
        {
            StopAllCoroutines();
            var attack = args.attack;
            _animator.CrossFadeInFixedTime("FE_Death", 0.001f);
            _animator.CrossFadeInFixedTime("GunGone", 0.001f);
            switch (attack.element)
            {
                case AttackElement.Concussive:
                    StartCoroutine(RagdollStateCoroutine(args));
                    break;
                default:
                    _animator.CrossFadeInFixedTime("Death", 0.01f);
                    break;
            }
            OnAimEnded?.Invoke(sender, args);
            OnDeath?.Invoke(this, EventArgs.Empty);
            enabled = false;
        };
        OnDashEnded += (sender, args) =>
        {
            StopCoroutine("DashTime");
            OnDashTrailEnd?.Invoke(this, EventArgs.Empty);
            if (OnGround())
            {
                ChangeState(_groundState);
            }
            else
            {
                _checkingForGround = true;
                if(_playerVars.prevState.GetType() != typeof(Jump) && _playerVars.prevState.GetType() != typeof(JumpEnded) && _playerVars.prevState.GetType() != typeof(Fall))
                    _playerVars.jumpCount++;
                ChangeState(_fallState);
            }
        };
    }

    private void MoveInput(InputAction.CallbackContext cxt)
    {
        var v2 = cxt.ReadValue<Vector2>();
        var dir = new Vector3(v2.x, 0, v2.y);
        if (_currentState == _groundState &&
            (!_animator.GetCurrentAnimatorStateInfo(0).IsName("Ground") &&
             !_animator.GetCurrentAnimatorStateInfo(0).IsName("Dash") &&
             !_animator.GetCurrentAnimatorStateInfo(0).IsName("GroundAim"))
            )
        {
            _animator.CrossFadeInFixedTime("Ground", 0.01f);
        }
        _playerVars.moveVector = dir;
    }

    private void JumpStarted(InputAction.CallbackContext cxt)
    {
        if (_playerVars.jumpCount < 2)
        {
            _playerVars.airborne = true;
            ChangeState(_jumpState);
            if(_playerVars.prevState == _dash)
                OnDashTrailEnd?.Invoke(this, EventArgs.Empty);
        }
    }

    private void JumpEnded(InputAction.CallbackContext cxt)
    {
        if(_currentState == _jumpState)
            ChangeState(_jumpEndedState);
    }

    private void DashStarted(InputAction.CallbackContext cxt)
    {
        StartCoroutine("DashTime");
    }

    private void DashEnded(InputAction.CallbackContext cxt)
    {
        if(_currentState == _dash)
            OnDashEnded?.Invoke(this, EventArgs.Empty);
    }

    private void ShootStarted(InputAction.CallbackContext cxt)
    {
        OnShootStarted?.Invoke(this, EventArgs.Empty);
    }

    private void ShootEnded(InputAction.CallbackContext cxt)
    {
        OnShootEnded?.Invoke(this, EventArgs.Empty);
    }

    private void SpringShoot(InputAction.CallbackContext cxt)
    {
        OnSpringShoot?.Invoke(this, EventArgs.Empty);
    }
    
    private void AimStarted(InputAction.CallbackContext cxt)
    {
        OnAimStarted?.Invoke(this, EventArgs.Empty);
    }

    private void AimEnded(InputAction.CallbackContext cxt)
    {
        OnAimEnded?.Invoke(this, EventArgs.Empty);
    }

    private void DetectiveInput(InputAction.CallbackContext ctx)
    {
        _detectiveActive = !_detectiveActive;
        _animator.SetBool("Detective", _detectiveActive);
    }

    private void InteractInput(InputAction.CallbackContext ctx)
    {
        OnInteract?.Invoke(this, EventArgs.Empty);
    }

    private void PauseInput(InputAction.CallbackContext ctx)
    {
        if (_paused)
        {
            _playerInput.Enable();
            _paused = false;
            OnUnpause?.Invoke(this, EventArgs.Empty);
            Time.timeScale = 1;
            Cursor.lockState = CursorLockMode.Locked;
        }
        else
        {
            _playerInput.Disable();
            _paused = true;
            OnPause?.Invoke(this, EventArgs.Empty);
            Time.timeScale = 0;
            Cursor.lockState = CursorLockMode.Confined;
        }
    }
    
    private bool OnGround()
    {
        var radius = _capsuleCollider.radius;
        var onGroundDistanceCheck = 0.1f;
        var floors = Physics.OverlapCapsule(transform.position - Vector3.up * onGroundDistanceCheck,
            transform.position + Vector3.up * onGroundDistanceCheck, radius, floorMask);

        return floors.Length > 0;
    }

    private IEnumerator WaitToGunIn()
    {
        yield return new WaitForSeconds(timeToSaveGun);
        if (!_playerVars.aiming)
        {
            OnGunIn?.Invoke(this, EventArgs.Empty);
        }
    }

    private IEnumerator DashTime()
    {
        OnDashTrailStart?.Invoke(this, EventArgs.Empty);
        ChangeState(_dash);
        yield return new WaitForSeconds(dashTime);
        if(_currentState == _dash)
            OnDashEnded?.Invoke(this, EventArgs.Empty);
    }

    private IEnumerator RagdollStateCoroutine(AttackArgs args)
    {
        var attack = args.attack;
        yield return new WaitForEndOfFrame();
        ChangeState(_ragdollState);
        _ragdollManager.ApplyForceAtPoint(attack.elementPoints, args.hitDirection, args.hitPoint);
        
    }

    private IEnumerator WaitForState(StateBase newState, float time)
    {
        yield return new WaitForSeconds(time);
        ChangeState(newState);
    }

    private IEnumerator WaitCoyoteTime()
    {
        _checkingForGround = true;
        yield return new WaitForSeconds(coyoteTime);
        if (!OnGround() && (_currentState == _groundState))
        {
            _playerVars.airborne = true;
            _playerVars.jumpCount++;
            ChangeState(_fallState);
        }
        else
            _checkingForGround = false;
    }

    private void ChangeState(StateBase newState)
    {
        _playerVars.prevState = _currentState;
        base.ChangeState(newState);
    }

    public void SetCheckingForGround()
    {
        _checkingForGround = true;
    }
}
