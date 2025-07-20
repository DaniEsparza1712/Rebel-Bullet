using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AttackWalk : StateBase
{
    private NavMeshAgent _agent;
    private Animator _animator;
    private float _speed;
    private SquadManager _squadManager;
    public EventHandler OnReachedTarget;
    
    public AttackWalk(Animator animator, NavMeshAgent agent, float speed, SquadManager squadManager) : base(animator)
    {
        _agent = agent;
        _animator = animator;
        _speed = speed;
        _squadManager = squadManager;
    }

    public override void Enter()
    {
        _animator.SetBool("AttackMode", true);
        _animator.CrossFadeInFixedTime("Run", 0.1f);
        _agent.speed = _speed;
        _agent.SetDestination(_squadManager.GetBestSpot(_agent.gameObject));
    }

    public override void UpdateState()
    {
        if (_agent.remainingDistance <= 0.3f)
            OnReachedTarget?.Invoke(this, EventArgs.Empty);
    }

    public override void UpdatePhysics()
    {
        
    }

    public override void Exit()
    {
        
    }
}
