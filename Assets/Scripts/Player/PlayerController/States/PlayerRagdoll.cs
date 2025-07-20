using System;
using UnityEngine;

public class PlayerRagdoll : StateBase
{
    private RagdollManager _ragdollManager;

    public EventHandler OnHipsSpeedZero;
    
    public PlayerRagdoll(Animator animator, RagdollManager ragdollManager) : base(animator)
    {
        _ragdollManager = ragdollManager;
    }

    public override void Enter()
    {
        _ragdollManager.EnableRagdoll();
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
    }
}
