using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Jump : StateBase
{
    private StateVars _stateVars;
    private Vector3 _moveDir;
    private float _speed;
    private float _dashMult;
    private Transform _transform;
    private Transform _camTransform;
    private CharacterController _cc;
    private float _rotationMod = 50;
    private float _decelaration;
    private float _minSpeed = 2;
    private float _jumpForce;

    private float _verticalSpeed;
    private Vector3 _lookDir;
    public EventHandler OnReachedMinSpeed;
        
    public Jump(Animator animator, float speed, float dashMultiplier, float force, float decelaration, StateVars movement, CharacterController cc) : base(animator)
    {
        _speed = speed;
        _transform = animator.transform;
        _cc = cc;
        _stateVars = movement;
        _jumpForce = force;
        _dashMult = dashMultiplier;
        _decelaration = decelaration;
            
        Anim = animator;
    }
    public override void Enter()
    {
        _stateVars.jumpCount++;
        
        if(_stateVars.jumpCount > 1)
            Anim.CrossFadeInFixedTime("JumpB", 0.1f);
        else
            Anim.CrossFadeInFixedTime("JumpA", 0.1f);
        Anim.CrossFadeInFixedTime("Angry", 0.1f);
        
        _camTransform = Camera.main.transform;
        _verticalSpeed = _jumpForce;

    }
        
    public override void UpdateState()
    {
        var dir = _stateVars.moveVector.normalized;

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
        var yVal = _verticalSpeed;
        yVal = Mathf.Max(_minSpeed, yVal - _decelaration * Time.fixedDeltaTime);
        _verticalSpeed = yVal;
        
        _lookDir = _moveDir.normalized;
        
        _cc.Move(_lookDir * (_speed * Time.fixedDeltaTime) + Vector3.up * (_verticalSpeed * Time.fixedDeltaTime));
        
        if(_moveDir != Vector3.zero)
            _transform.rotation =
                Quaternion.RotateTowards(_transform.rotation, Quaternion.LookRotation(_lookDir), 2);
        
        if(_verticalSpeed <= _minSpeed)
            OnReachedMinSpeed?.Invoke(this, EventArgs.Empty);
    }
    
    public override void Exit()
    {
    }
}
