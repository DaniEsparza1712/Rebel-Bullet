using System;
using Unity.AI.Navigation;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class ChaseState : StateBase
{
    private NavMeshAgent _agent;
    private Animator _animator;
    private RoboVars _roboVars;
    private float _speed;
    private float _timer;
    public EventHandler OnReachDestination;
    
    public ChaseState(Animator animator, NavMeshAgent agent, RoboVars roboVars, float speed) : base(animator)
    {
        _agent = agent;
        _animator = animator;
        _roboVars = roboVars;
        _speed = speed;

        OnReachDestination += (sender, args) =>
        {
            Debug.Log("Reached the destination");
        };
    }

    public override void Enter()
    {
        _timer = 0;
        NavMesh.SamplePosition(_roboVars.chasePos, out var hit, 5, NavMesh.AllAreas);
        _agent.SetDestination(hit.position);
        _animator.CrossFadeInFixedTime("Run", 0.1f);
        _agent.speed = _speed;
    }

    public override void UpdateState()
    {
        if (_timer <= 0.2f)
        {
            _timer += Time.deltaTime;
            return;
        }
        if(_agent.remainingDistance <= _agent.stoppingDistance + Mathf.Epsilon)
            OnReachDestination?.Invoke(this, EventArgs.Empty);
    }

    public override void UpdatePhysics()
    {
        
    }

    public override void Exit()
    {
        
    }
}
