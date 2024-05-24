using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NimbledroneIdle : State
{
    private NimbledroneMachine _sm;
    public NimbledroneIdle(NimbledroneMachine nm) : base(nm)
    {
        _sm = nm;
    }

    public override void Enter()
    {
        _sm.Agent.stoppingDistance = 0;
        _sm.bulletManager.isShooting = false;
    }

    public override void UpdateLogic()
    {
        if (_sm.DistanceToTarget() <= _sm.sightDistance)
        {
            _sm.ChangeState(_sm.Follow);
        }
    }

    public override void FixedUpdateLogic()
    {
        _sm.Agent.SetDestination(_sm.transform.position);
    }
}
