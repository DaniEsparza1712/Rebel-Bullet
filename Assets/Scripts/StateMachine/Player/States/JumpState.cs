using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpState : State
{
    public PlayerMachine sm;
    float targetHeight;
    float currentJumpVel;
    Vector3 direction;
    float viewAngle;
    Vector3 viewDir;
    float hAxis;
    float vAxis;
    float rotationMod = 50;
    Vector3 headPos;
    float jumpGrace;
    public JumpState(PlayerMachine pm): base(pm){
        sm = pm;
    }

    public override void Enter()
    {
        sm.rb.isKinematic = false;
        sm.mainCollider.enabled = true;
        sm.character.enabled = false;
        
        sm.ik_Controller.pointAt = false;
        sm.rig.weight = 0;

        sm.animator.SetTrigger("Jump");
        sm.animator.SetTrigger("FE_Angry");
        sm.animator.ResetTrigger("Base");

        sm.lockOnIcon.SetActive(false);
        sm.weaponAppear.Disappear();
        
        sm.rb.MovePosition(sm.transform.position + (sm.character.radius/2 + 0.2f) * sm.transform.up);
        sm.rb.AddForce(sm.transform.up * sm.jumpForce, ForceMode.Impulse);
        
        UnityEngine.Object.Instantiate(sm.jumpDust, sm.transform.position, sm.jumpDust.transform.rotation);
    }
    public override void UpdateLogic()
    {
        
        if (sm.OnGround() && sm.hasJumped)
        {
            sm.ChangeState(sm.Land);
            if(sm.currentState != this)
                sm.hasJumped = false;
        }
        else if (Input.GetAxisRaw("Lock On") == 1 || Input.GetButtonDown("Lock On"))
        {
            sm.hasJumped = false;
            sm.ChangeState(sm.lockOnFall);
        }
        else if (Input.GetButton("Fire1"))
        {
            sm.hasJumped = false;
            sm.ChangeState(sm.lockOnFall);
        }

        hAxis = Input.GetAxis("Horizontal");
        vAxis = Input.GetAxis("Vertical");
        direction = new Vector3(hAxis, 0, vAxis);

        viewAngle = -Camera.main.transform.eulerAngles.y;
        viewDir = MathUtilities.TurnVector(direction, viewAngle);
    }
    public override void FixedUpdateLogic() {
        sm.rb.AddForce(viewDir * (sm.jumpVel), ForceMode.VelocityChange);
        if (viewDir.magnitude > 0 + Mathf.Epsilon)
            sm.transform.rotation = Quaternion.RotateTowards(sm.transform.rotation, Quaternion.LookRotation(viewDir), sm.speed * rotationMod);
    }
}
