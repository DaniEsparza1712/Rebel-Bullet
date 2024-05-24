using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossDeath : State
{
    private BossMachine _sm;
    public BossDeath(BossMachine bm) : base(bm)
    {
        _sm = bm;
    }

    public override void Enter()
    {
        _sm.lifeSystem.SetRegen(0, 0);
        _sm.Animator.SetTrigger("Death");
    }
    public override void UpdateLogic()
    {
        
    }
    public override void FixedUpdateLogic()
    {
        
    }
}
