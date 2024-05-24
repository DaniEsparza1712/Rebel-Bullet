using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NimbledroneShoot : State
{
    private NimbledroneMachine _sm;
    private Vector3 fwd;

    public NimbledroneShoot(NimbledroneMachine nm) : base(nm)
    {
        _sm = nm;
    }
    public override void Enter()
    {
        _sm.Agent.speed = 0.5f;
        _sm.Agent.stoppingDistance = 1;
        _sm.bulletManager.isShooting = true;
    }

    public override void UpdateLogic()
    {
        _sm.Agent.SetDestination(_sm.target.position);
        if (_sm.DistanceToTarget() >= _sm.shootingDistance)
        {
            _sm.ChangeState(_sm.Follow);
        }
    }

    public override void FixedUpdateLogic()
    {
        /*fwd = (_sm.target.position - _sm.transform.position);
        fwd.y = _sm.transform.position.y;
        _sm.transform.forward = fwd.normalized;*/
    }
}
