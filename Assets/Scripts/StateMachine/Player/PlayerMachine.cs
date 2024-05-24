using System;
using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.Animations.Rigging;
using UnityEngine.XR;

public class PlayerMachine : StateMachine
{
    [Header("External Components")]
    public CharacterController character;
    public Rigidbody rb;
    public CapsuleCollider mainCollider;
    public Animator animator;
    public CameraSwitcher cameraSwitcher;
    public LockOnSystem lockOnSystem;
    public WeaponAppearController weaponAppear;
    public IK_Controller ik_Controller;
    public Rig rig;
    public GameObject trailObject;
    
    [Header("Walking data")]
    public float baseSpeed;
    public float dashSpeed;
    public float dashTime;

    [Header("Jump Data")]
    public float jumpForce;
    public float jumpVel;

    [Header("Layers")]
    public LayerMask floorMask;
    public LayerMask enemyMask;

    [Header("VFX")]
    public ParticleSystem jumpDust;

    [Header("Target")]
    public Transform lockOnTr;

    [Header("UI")]
    public GameObject lockOnIcon;

    [HideInInspector]
    public float speed;
    [HideInInspector]
    public BaseState baseState;
    [HideInInspector]
    public JumpState jumpState;
    [HideInInspector]
    public FallState fallState;
    [HideInInspector]
    public LockOnBaseState lockOnBase;
    [HideInInspector]
    public LockOnJumpState lockOnJump;
    [HideInInspector]
    public LockOnFall lockOnFall;
    [HideInInspector] 
    public Death Death;
    [HideInInspector] 
    public LandState Land;
    [HideInInspector] 
    public AttackState AttackState;
    [HideInInspector] 
    public AttackHState AttackHState;
    [HideInInspector] 
    public DashState DashState;
    [HideInInspector] 
    public LockOnDashState LockOnDashState;
    
    float onGroundDistanceCheck;
    public TMP_Text StateText;

    [HideInInspector] public string changeTo;
    [HideInInspector] public bool hasJumped = false;
    private void Awake() {
        baseState = new BaseState(this);
        jumpState = new JumpState(this);
        fallState = new FallState(this);
        lockOnBase = new LockOnBaseState(this);
        lockOnJump = new LockOnJumpState(this);
        lockOnFall = new LockOnFall(this);
        Death = new Death(this);
        Land = new LandState(this);
        AttackState = new AttackState(this);
        AttackHState = new AttackHState(this);
        DashState = new DashState(this);
        LockOnDashState = new LockOnDashState(this);
        trailObject.SetActive(false);

        character.enabled = false;
        currentState = baseState;
        Cursor.lockState = CursorLockMode.Locked;
    }
    public bool OnGround()
    {
        var radius = mainCollider.radius;
        onGroundDistanceCheck = 0f;
        var floors = Physics.OverlapCapsule(transform.position - Vector3.up * onGroundDistanceCheck,
            transform.position + Vector3.up * onGroundDistanceCheck, radius, floorMask);
        if (floors.Length > 0)
            return true;
        return false;
    }
    public void ForceFaceForward()
    {
        Vector3 direction = new Vector3(0, 0, 1);
        float viewAngle = -Camera.main.transform.eulerAngles.y;
        Vector3 viewDir = MathUtilities.TurnVector(direction, viewAngle);
        transform.rotation = Quaternion.LookRotation(viewDir);
    }
    public IEnumerator SnapCamera()
    {
        yield return new WaitUntil(CheckMainCameraEnabled);
        yield return new WaitForFixedUpdate();
        Vector3 vectorA = -transform.forward.normalized;
        Vector3 vectorB = (cameraSwitcher.mainCamera.transform.position - transform.position).normalized;
        vectorA.y = 0;
        vectorB.y = 0;

        float angle;
        Vector3 rightQuadrantPos = transform.position + transform.right.normalized * 3;
        Vector3 leftQuadrantPos = transform.position - transform.right.normalized * 3;
        if (Vector3.Distance(cameraSwitcher.mainCamera.transform.position, rightQuadrantPos) < Vector3.Distance(cameraSwitcher.mainCamera.transform.position, leftQuadrantPos))
        {
            angle = Vector3.Angle(vectorA, vectorB);
        }
        else
        {
            angle = 360-Vector3.Angle(vectorA, vectorB);
        }
        cameraSwitcher.mainCamera.m_XAxis.Value = angle;
    }

    public void BackToIdle()
    {
        animator.SetInteger("Attack", 0);
        animator.SetInteger("AttackH", 0);
        changeTo = "idle";
    }
    bool CheckMainCameraEnabled()
    {
        return !cameraSwitcher.brain.IsBlending;
    }

    public void DeathEvent()
    {
        ChangeState(Death);
    }

    public void CanChain(int chain)
    {
        chain = Math.Clamp(chain, 0, 1);
        if(chain > 0)
            animator.SetBool("CanChain", true);
        else
            animator.SetBool("CanChain", false);
    }

    public void JumpEvent()
    {
        hasJumped = true;
    }
}
