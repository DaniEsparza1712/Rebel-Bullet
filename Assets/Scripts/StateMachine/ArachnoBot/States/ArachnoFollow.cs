using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArachnoFollow : State
{
    private ArachnoBotMachine _sm;
    private float _minDistance;
    private Vector3 _currentDistance;

    public ArachnoFollow(ArachnoBotMachine am) : base(am)
    {
        _sm = am;
    }

    public override void Enter()
    {
        _sm.agent.speed = _sm.followSpeed;
        _sm.agent.destination = _sm.target.transform.position;
        _minDistance = 2;
        
        _sm.Animator.SetTrigger("Idle");
    }

    public override void UpdateLogic()
    {
        _currentDistance = _sm.target.transform.position - _sm.transform.position;
        if (_sm.changeTo == "KO")
        {
            _sm.changeTo = "";
            _sm.ChangeState(_sm.ko);
        }
        else if (_currentDistance.magnitude <= _minDistance)
        {
            _sm.ChangeState(_sm.shooting);
        }
    }

    public override void FixedUpdateLogic()
    {
        _sm.agent.destination = _sm.target.transform.position;
    }
}
