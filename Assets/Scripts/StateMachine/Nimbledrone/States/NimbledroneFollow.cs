using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class NimbledroneFollow : State
{
    private NimbledroneMachine _sm;
    public NimbledroneFollow(NimbledroneMachine nm) : base(nm)
    {
        _sm = nm;
    }

    public override void Enter()
    {
        _sm.Agent.speed = _sm.speed;
        _sm.Agent.stoppingDistance = 0;
        _sm.bulletManager.isShooting = false;
    }

    public override void UpdateLogic()
    {
        if (_sm.DistanceToTarget() <= _sm.Distance)
        {
            _sm.ChangeState(_sm.Shoot);
        }
    }

    public override void FixedUpdateLogic()
    {
        _sm.Agent.SetDestination(GetStoppingPoint());
    }

    private Vector3 GetStoppingPoint()
    {
        var pos = _sm.transform.position;
        var targetPos = _sm.target.position;
        var dir = pos - targetPos;
        dir.Normalize();
        
        return targetPos + (dir * _sm.Distance);
    }
    
}
