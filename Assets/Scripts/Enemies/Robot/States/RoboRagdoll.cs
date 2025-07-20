using System;
using UnityEngine;
using UnityEngine.AI;

public class RoboRagdoll : StateBase
{
    private NavMeshAgent _agent;
    private Animator _animator;
    private RagdollManager _ragdollManager;
    private RoboVars _roboVars;

    public EventHandler OnHipsSpeedZero;
    
    public RoboRagdoll(Animator animator, NavMeshAgent agent, RagdollManager ragdollManager, RoboVars vars) : base(animator)
    {
        _agent = agent;
        _animator = animator;
        _ragdollManager = ragdollManager;
        _roboVars = vars;
    }

    public override void Enter()
    {
        _agent.enabled = false;
        _ragdollManager.EnableRagdoll();
        _roboVars.patrolling = false;
    }

    public override void UpdateState()
    {
        if(_ragdollManager.CanGetUp())
            OnHipsSpeedZero?.Invoke(this, EventArgs.Empty);
    }

    public override void UpdatePhysics()
    {
        
    }

    public override void Exit()
    {
        _ragdollManager.UpdateTransform();
        _ragdollManager.DisableRagdoll();
        _agent.enabled = true;
        _roboVars.patrolling = true;
    }
}
