using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class LockOnFall : State
{
    enum LockMode
    {
        Free,
        Target
    }
    LockMode lockMode;
    PlayerMachine sm;
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
    float newYRotation;
    private float _rotationMod;
    Vector3 boxCheckDimensions = new Vector3(6, 6, 9);
    float fallVelocity;
    Vector3 headPos;
    float onGroundDistanceCheck;
    float targetHeight;
    public LockOnFall(PlayerMachine pm) : base(pm)
    {
        sm = pm;
    }

    public override void Enter()
    {
        _rotationMod = sm.lockOnSystem.rotationMod;
        sm.ik_Controller.pointAt = true;
        sm.rig.weight = 1;
        sm.lockOnTr.forward = Camera.main.transform.forward;
        
        sm.rb.isKinematic = false;
        sm.mainCollider.enabled = true;
        sm.character.enabled = false;
        
        if (sm.cameraSwitcher.mainCamera.enabled)
            sm.ForceFaceForward();

        sm.cameraSwitcher.SwitchCamera(CameraSwitcher.Cameras.lockOn);
        sm.lockOnIcon.SetActive(true);
        sm.weaponAppear.Appear();

        newXRotation = sm.lockOnSystem.currentXRotation;
        newYRotation = sm.transform.eulerAngles.y;
        sm.animator.SetTrigger("Fall");
        sm.animator.ResetTrigger("Jump");
    }
    public override void UpdateLogic()
    {
        hAxis = Input.GetAxis("Horizontal");
        vAxis = Input.GetAxis("Vertical");

        direction = new Vector3(hAxis, 0, vAxis);

        sm.animator.SetFloat("Lock On X", hAxis);
        sm.animator.SetFloat("Lock On Y", vAxis);

        viewAngle = -Camera.main.transform.eulerAngles.y;

        viewDir = MathUtilities.TurnVector(direction, viewAngle);

        if (lockMode == LockMode.Free)
        {
            hAxisB = Input.GetAxis("Horizontal B");
            vAxisB = Input.GetAxis("Vertical B");

            directionB = new Vector3(vAxisB, hAxisB, 0);

            newYRotation = sm.transform.eulerAngles.y + directionB.y * sm.speed * _rotationMod;
            newXRotation += directionB.x * sm.speed * _rotationMod;
            sm.lockOnSystem.currentXRotation = newXRotation;
            newXRotation = Mathf.Clamp(newXRotation, sm.lockOnSystem.minXRotation, sm.lockOnSystem.maxXRotation);
        }
        else
        {
            hAxisB = Input.GetAxisRaw("Horizontal B");
        }

        if ((Input.GetAxisRaw("Lock On") <= 0 && !Input.GetButton("Fire1")) || (Input.GetButtonUp("Lock On") && !Input.GetButton("Fire1")))
        {
            sm.transform.rotation = Quaternion.Euler(0, sm.transform.eulerAngles.y, 0);
            sm.ChangeState(sm.fallState);
        }
        if (sm.OnGround())
        {
            sm.ChangeState(sm.lockOnBase);
        }
    }

    public override void FixedUpdateLogic()
    {
        sm.rb.AddForce(viewDir * (sm.jumpVel), ForceMode.VelocityChange);
        
        float x = sm.lockOnTr.transform.localEulerAngles.x + directionB.x * sm.speed * _rotationMod;
        float y = sm.lockOnTr.transform.localEulerAngles.y + directionB.y * sm.speed * _rotationMod;
        Vector3 shoulderRot = new Vector3(x, y, 0);
        shoulderRot = Quaternion.Euler(shoulderRot).eulerAngles;
        if (shoulderRot.x < 315 && shoulderRot.x > 300)
            shoulderRot.x = 315;
        else if (shoulderRot.x > 45 && shoulderRot.x < 60)
            shoulderRot.x = 45;
        sm.lockOnTr.transform.localEulerAngles = shoulderRot;

        Quaternion bodyRot = Quaternion.Euler(0, sm.lockOnTr.transform.eulerAngles.y, 0);
        sm.transform.rotation = Quaternion.RotateTowards(sm.transform.rotation, bodyRot, _rotationMod);
    }
}
