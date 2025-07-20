using System;
using UnityEngine;
using UnityEngine.AI;

public class AttackAim : StateBase
{
    private NavMeshAgent _agent;
    private NavMeshObstacle _obstacle;
    private Animator _animator;
    private Transform _target;
    private float _rangeDistance;
    private float _rotOffset;
    public EventHandler OnRangePassed;
    public EventHandler OnStopAiming;
    
    public AttackAim(Animator animator, NavMeshAgent agent, NavMeshObstacle obstacle, Transform target, float range, float rotOffset) : base(animator)
    {
        _agent = agent;
        _obstacle = obstacle;
        _animator = animator;
        _target = target;
        _rangeDistance = range;
        _rotOffset = rotOffset;
    }

    public override void Enter()
    {
        _animator.CrossFadeInFixedTime("Aim", 0.1f);
        _agent.enabled = false;
        _obstacle.enabled = true;
    }

    public override void UpdateState()
    {
        if(Vector3.Distance(_agent.transform.position, _target.position) > _rangeDistance)
            OnRangePassed?.Invoke(this, EventArgs.Empty);
    }

    public override void UpdatePhysics()
    {
        var targetPos = _target.position;
        targetPos.y = _agent.transform.position.y;
        var dir = (targetPos - _agent.transform.position).normalized;
        dir = Quaternion.AngleAxis(_rotOffset, Vector3.up) * dir;
        var rot = Quaternion.RotateTowards(_agent.transform.rotation, 
            Quaternion.LookRotation(dir), 15);
        _agent.transform.rotation = rot; 
    }

    public override void Exit()
    {
        _obstacle.enabled = false;
        _agent.enabled = true;
        OnStopAiming?.Invoke(this, EventArgs.Empty);
    }
}
