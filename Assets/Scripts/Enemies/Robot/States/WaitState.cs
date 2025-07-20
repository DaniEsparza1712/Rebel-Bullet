using UnityEngine;
using UnityEngine.AI;

public class WaitState : StateBase
{
    private NavMeshAgent _agent;
    private Animator _animator;
    private NavMeshObstacle _obstacle;
    
    public WaitState(Animator animator, NavMeshAgent agent, NavMeshObstacle obstacle) : base(animator)
    {
        _agent = agent;
        _animator = animator;
        _obstacle = obstacle;
    }

    public override void Enter()
    {
        _animator.CrossFadeInFixedTime("Idle", 0.1f);
        _agent.enabled = false;
        _obstacle.enabled = true;
    }

    public override void UpdateState()
    {
        
    }

    public override void UpdatePhysics()
    {
        
    }

    public override void Exit()
    {
        _obstacle.enabled = false;
        _agent.enabled = true;
    }
}
