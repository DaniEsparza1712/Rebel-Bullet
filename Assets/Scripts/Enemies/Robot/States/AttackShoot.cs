using System;
using UnityEngine;
using UnityEngine.AI;

public class AttackShoot : StateBase
{
    private NavMeshAgent _agent;
    private Animator _animator;
    public EventHandler OnStopShooting;
    
    public AttackShoot(Animator animator, NavMeshAgent agent) : base(animator)
    {
        _agent = agent;
        _animator = animator;
    }

    public override void Enter()
    {
        _animator.CrossFadeInFixedTime("Shoot", 0.1f);
        _agent.speed = 0;
        _agent.updateRotation = false;
        _agent.SetDestination(_agent.transform.position);
    }

    public override void UpdateState()
    {
        
    }

    public override void UpdatePhysics()
    {
        
    }

    public override void Exit()
    {
        _agent.updateRotation = true;
        OnStopShooting?.Invoke(this, EventArgs.Empty);
    }
}
