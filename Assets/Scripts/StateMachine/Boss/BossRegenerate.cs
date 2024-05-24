using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossRegenerate : State
{
    private BossMachine _sm;
    public BossRegenerate(BossMachine bm) : base(bm)
    {
        _sm = bm;
    }

    public override void Enter()
    {
        _sm.lifeSystem.SetRegen(_sm.regenerationRate, _sm.regenerationValue);
        _sm.Animator.SetTrigger("Regenerate");
    }
    public override void UpdateLogic()
    {
        if(_sm.changeTo == "Death")
        {
            _sm.ChangeState(_sm.Death);
        }
        else if (_sm.changeTo == "Idle")
        {
            _sm.ChangeState(_sm.Idle);
            _sm.lifeSystem.SetRegen(0, 0);
            if(_sm.currentState != this)
                _sm.ChangeTo("");
        }
    }
    public override void FixedUpdateLogic()
    {
        
    }
}
