using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

public class PatrolState : StateBase
{
    private NavMeshAgent _agent;
    private Animator _animator;
    private List<Transform> _waypoints;
    private Vector3 _destination;
    private float _speed;
    public EventHandler OnReachDestination;
    
    public PatrolState(Animator animator, NavMeshAgent agent, List<Transform> waypoints, float speed) : base(animator)
    {
        _agent = agent;
        _animator = animator;
        _waypoints = waypoints;
        _speed = speed;
    }

    public override void Enter()
    {
        _animator.CrossFadeInFixedTime("Walk", 0.1f);
        _destination = _waypoints[Random.Range(0, _waypoints.Count)].position;
        _agent.speed = _speed;
        _agent.SetDestination(_destination);
    }

    public override void UpdateState()
    {
        //Debug.Log($"RD: {_agent.remainingDistance}; SD: {_agent.stoppingDistance + Mathf.Epsilon}");
        if(_agent.remainingDistance <= _agent.stoppingDistance + 0.01f)
            OnReachDestination?.Invoke(this, EventArgs.Empty);
    }

    public override void UpdatePhysics()
    {
        
    }

    public override void Exit()
    {
        
    }
}
