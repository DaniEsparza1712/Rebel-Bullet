using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArachnoShooting : State
{
    private ArachnoBotMachine _sm;

    public ArachnoShooting(ArachnoBotMachine am) : base(am)
    {
        _sm = am;
    }

    public override void Enter()
    {
        _sm.agent.speed = _sm.shootingSpeed;
        _sm.Animator.SetTrigger("Attack");
    }

    public override void UpdateLogic()
    {
        if (_sm.changeTo == "KO")
        {
            _sm.changeTo = "";
            _sm.ChangeState(_sm.ko);
        }
    }

    public override void FixedUpdateLogic()
    {
        _sm.agent.destination = _sm.target.transform.position;
    }
}
