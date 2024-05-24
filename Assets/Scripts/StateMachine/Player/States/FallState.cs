using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallState : State
{
    PlayerMachine sm;
    Vector3 direction;
    float viewAngle;
    Vector3 viewDir;
    float hAxis;
    float vAxis;
    float rotationMod = 50;
    public FallState(PlayerMachine pm): base(pm){
        sm = pm;
    }

    public override void Enter(){
        sm.animator.SetTrigger("Fall");
        sm.ik_Controller.pointAt = false;
        sm.rig.weight = 0;
        
        sm.rb.isKinematic = false;
        sm.mainCollider.enabled = true;
        sm.character.enabled = false;

        sm.lockOnIcon.SetActive(false);
        sm.weaponAppear.Disappear();

        if (sm.cameraSwitcher.lockOnCamera.enabled)
        {
            sm.StartCoroutine(sm.SnapCamera());
            sm.cameraSwitcher.SwitchCamera(CameraSwitcher.Cameras.main);
        }
        else
            sm.cameraSwitcher.SwitchCamera(CameraSwitcher.Cameras.main);
    }
    public override void UpdateLogic(){
        
        if(sm.OnGround()){
            sm.ChangeState(sm.Land);
        }
        else if (Input.GetAxisRaw("Lock On") == 1 || Input.GetButtonDown("Lock On"))
        {
            sm.ChangeState(sm.lockOnFall);
        }
        else if (Input.GetButton("Fire1") || Input.GetAxisRaw("Lock On") == 1)
        {
            sm.ChangeState(sm.lockOnFall);
        }

        hAxis = Input.GetAxis("Horizontal");
        vAxis = Input.GetAxis("Vertical");
        direction = new Vector3(hAxis, 0, vAxis);
        sm.animator.SetFloat("Walk", direction.magnitude);

        viewAngle = -Camera.main.transform.eulerAngles.y;

        viewDir = MathUtilities.TurnVector(direction, viewAngle);
    }
    public override void FixedUpdateLogic(){
        sm.rb.AddForce(viewDir * (sm.jumpVel), ForceMode.VelocityChange);
        if(viewDir.magnitude > 0 + Mathf.Epsilon)
            sm.transform.rotation = Quaternion.RotateTowards(sm.transform.rotation, Quaternion.LookRotation(viewDir), sm.speed * rotationMod);
    }
}
