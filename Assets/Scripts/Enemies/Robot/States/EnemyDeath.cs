using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyDeath : StateBase
{
    private NavMeshAgent _agent;
    private Animator _animator;
    
    public EnemyDeath(Animator animator, NavMeshAgent agent) : base(animator)
    {
        _agent = agent;
        _animator = animator;
    }

    public override void Enter()
    {
        _animator.CrossFadeInFixedTime("Death", 0.1f);
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
    }
}
