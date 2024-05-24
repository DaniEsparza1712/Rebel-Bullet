using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Death : State
{
    private PlayerMachine sm;

    public Death(PlayerMachine pm) : base(pm)
    {
        sm = pm;
    }

    public override void Enter()
    {
        sm.animator.SetTrigger("FE_Surprised");
        sm.lockOnIcon.SetActive(false);
        sm.ik_Controller.pointAt = false;
        sm.ik_Controller.lookAt = false;
        sm.weaponAppear.Disappear();
        sm.rig.weight = 0;
        
        sm.cameraSwitcher.SwitchCamera(CameraSwitcher.Cameras.death);
    }

    public override void UpdateLogic()
    {
        
    }

    public override void FixedUpdateLogic()
    {
        
    }
}
