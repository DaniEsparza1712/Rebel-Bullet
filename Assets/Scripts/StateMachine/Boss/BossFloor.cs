using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossFloor : State
{
    private BossMachine _sm;
    public BossFloor(BossMachine bm) : base(bm)
    {
        _sm = bm;
    }

    public override void Enter()
    {
        _sm.Animator.SetTrigger("Floor");
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
            if(_sm.currentState != this)
                _sm.ChangeTo("");
        }
    }
    public override void FixedUpdateLogic()
    {
        
    }
}
