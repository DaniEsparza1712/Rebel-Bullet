using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AimState : StateBase
{
    private StateVars _stateVars;
    private Vector3 _moveDir;
    private float _speed;
    private Transform _transform;
    private Transform _camTransform;
        
    public AimState(Animator animator, float speed, StateVars movement) : base(animator)
    {
        _speed = speed;
        _transform = animator.transform;
        _stateVars = movement;
            
        Anim = animator;
    }
    public override void Enter()
    {
        Anim.CrossFadeInFixedTime("GroundAim", 0.1f);
        Anim.CrossFadeInFixedTime("GunAppear", 0.1f);
        Anim.CrossFadeInFixedTime("Angry", 0.1f);
        Anim.CrossFadeInFixedTime("ExtendL", 0.1f);
        Anim.CrossFadeInFixedTime("HoldR", 0.1f);
        _camTransform = Camera.main.transform;
    }
        
    public override void UpdateState()
    {
        var dir = _stateVars.moveVector;
        Anim.SetFloat("WalkX", dir.x);
        Anim.SetFloat("WalkY", dir.z);

        var camFwd = _camTransform.forward;
        var camRight = _camTransform.right;
            
        camFwd.y = 0;
        camRight.y = 0;
        camFwd.Normalize();
        camRight.Normalize();
            
        _moveDir = camFwd * dir.z + camRight * dir.x;
    }
        
    public override void UpdatePhysics()
    {
        var rot = _transform.eulerAngles;
        rot.y = _camTransform.eulerAngles.y;
        _transform.rotation = Quaternion.Euler(rot);
    }

    public override void Exit()
    {
        Anim.CrossFadeInFixedTime("GunDisappear", 0.1f);
        Anim.CrossFadeInFixedTime("Neutral", 0.1f);
        Anim.CrossFadeInFixedTime("EmptyL", 0.1f);
        Anim.CrossFadeInFixedTime("EmptyR", 0.1f);
    }
}
