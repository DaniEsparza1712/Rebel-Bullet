using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EmptyState : StateBase
{
    private Animator _animator;
    private string _animName;
    
    public EmptyState(Animator animator, string anim) : base(animator)
    {
        _animator = animator;
        _animName = anim;
    }

    public override void Enter()
    {
        _animator.CrossFadeInFixedTime(_animName, 0.1f);
    }

    public override void UpdateState()
    {
        
    }

    public override void UpdatePhysics()
    {
        
    }

    public override void Exit()
    {
        
    }
}
