using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class LockOnJumpState : State
{
    enum LockMode
    {
        Free,
        Target
    }
    private LockMode _lockMode;
    private PlayerMachine _sm;
    Vector3 direction;
    Vector3 directionB;
    float viewAngle;
    Vector3 viewDir;
    float hAxis;
    float vAxis;
    float hAxisB;
    float vAxisB;
    float scroll;
    float newXRotation;
    private float _rotationMod;
    float currentJumpVel;
    float jumpGrace;
    float onGroundDistanceCheck;
    float targetHeight;
    public LockOnJumpState(PlayerMachine pm) : base(pm)
    {
        _sm = pm;
    }

    public override void Enter()
    {
        _rotationMod = _sm.lockOnSystem.rotationMod;
        _sm.rb.isKinematic = false;
        _sm.mainCollider.enabled = true;
        _sm.character.enabled = false;
        
        _sm.ik_Controller.pointAt = true;
        _sm.rig.weight = 1;
        _sm.lockOnTr.forward = Camera.main.transform.forward;

        if (_sm.cameraSwitcher.mainCamera.enabled)
            _sm.ForceFaceForward();

        _sm.cameraSwitcher.SwitchCamera(CameraSwitcher.Cameras.lockOn);
        _sm.lockOnIcon.SetActive(true);
        _sm.weaponAppear.Appear();

        newXRotation = _sm.lockOnSystem.currentXRotation;
        _sm.animator.SetTrigger("Jump");
        _sm.animator.ResetTrigger("LockOn");

        _sm.rb.MovePosition(_sm.transform.position + (_sm.character.radius/2 + 0.2f) * _sm.transform.up);
        _sm.rb.AddForce(_sm.transform.up * _sm.jumpForce, ForceMode.Impulse);
        
        Object.Instantiate(_sm.jumpDust, _sm.transform.position, _sm.jumpDust.transform.rotation);
    }
    public override void UpdateLogic()
    {
        hAxis = Input.GetAxis("Horizontal");
        vAxis = Input.GetAxis("Vertical");

        direction = new Vector3(hAxis, 0, vAxis);

        _sm.animator.SetFloat("Lock On X", hAxis);
        _sm.animator.SetFloat("Lock On Y", vAxis);

        viewAngle = -Camera.main.transform.eulerAngles.y;

        viewDir = MathUtilities.TurnVector(direction, viewAngle);

        if (_lockMode == LockMode.Free)
        {
            hAxisB = Input.GetAxis("Horizontal B");
            vAxisB = Input.GetAxis("Vertical B");

            directionB = new Vector3(vAxisB, hAxisB, 0);
            
            newXRotation += directionB.x * _sm.speed * _rotationMod;
            _sm.lockOnSystem.currentXRotation = newXRotation;
            newXRotation = Mathf.Clamp(newXRotation, _sm.lockOnSystem.minXRotation, _sm.lockOnSystem.maxXRotation);
        }
        else
        {
            hAxisB = Input.GetAxisRaw("Horizontal B");
        }
        
        if ((Input.GetAxisRaw("Lock On") <= 0 && !Input.GetButton("Fire1")) || (Input.GetButtonUp("Lock On") && Input.GetAxisRaw("Fire1") != 1))
        {
            _sm.hasJumped = false;
            _sm.transform.rotation = Quaternion.Euler(0, _sm.transform.eulerAngles.y, 0);
            _sm.ChangeState(_sm.fallState);
        }
        else if (_sm.OnGround() && _sm.hasJumped)
        {
            _sm.hasJumped = false;
            _sm.ChangeState(_sm.lockOnBase);
        }
    }

    public override void FixedUpdateLogic()
    {
        _sm.rb.AddForce(viewDir * (_sm.jumpVel), ForceMode.VelocityChange);
        
        float x = _sm.lockOnTr.transform.localEulerAngles.x + directionB.x * _sm.speed * _rotationMod;
        float y = _sm.lockOnTr.transform.localEulerAngles.y + directionB.y * _sm.speed * _rotationMod;
        Vector3 shoulderRot = new Vector3(x, y, 0);
        shoulderRot = Quaternion.Euler(shoulderRot).eulerAngles;
        if (shoulderRot.x < 315 && shoulderRot.x > 300)
            shoulderRot.x = 315;
        else if (shoulderRot.x > 45 && shoulderRot.x < 60)
            shoulderRot.x = 45;
        _sm.lockOnTr.transform.localEulerAngles = shoulderRot;

        Quaternion bodyRot = Quaternion.Euler(0, _sm.lockOnTr.transform.eulerAngles.y, 0);
        _sm.transform.rotation = Quaternion.RotateTowards(_sm.transform.rotation, bodyRot, _rotationMod);
    }
}
