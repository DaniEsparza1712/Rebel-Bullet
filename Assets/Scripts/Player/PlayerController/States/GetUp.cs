using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetUp : StateBase
{
    private RagdollManager _ragdollManager;
    private Animator _animator;
    private CharacterController _characterController;
    
    public GetUp(Animator animator, RagdollManager ragdollManager, CharacterController characterController) : base(animator)
    {
        _animator = animator;
        _ragdollManager = ragdollManager;
        _characterController = characterController;
    }

    public override void Enter()
    {
        _animator.CrossFadeInFixedTime("GetUp", 0.1f);
    }

    public override void UpdateState()
    {
        
    }

    public override void UpdatePhysics()
    {
        _characterController.Move(Physics.gravity * 0.02f);
    }

    public override void Exit()
    {
        
    }
}
