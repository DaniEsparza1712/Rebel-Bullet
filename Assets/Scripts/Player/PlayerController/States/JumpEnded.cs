using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpEnded : StateBase
{
    private StateVars _stateVars;
    private Vector3 _moveDir;
    private Vector3 _moveVelocity;
    private Vector3 _lookDir;
    
    private float _speed;
    private Transform _camTransform;
    private CharacterController _characterController;
    private float _rotationMod = 50;
    private float _deceleration;
    private float _minSpeed = 2;
    private float _verticalVelocity;
    private Transform _transform;
    public EventHandler OnReachedZero;
        
    public JumpEnded(Animator animator, float speed, float deceleration, StateVars movement, Rigidbody rb, CapsuleCollider collider, CharacterController controller) : base(animator)
    {
        _speed = speed;
        _stateVars = movement;
        
        _characterController = controller;
        
        _deceleration = deceleration;
        _transform = _characterController.transform;
            
        Anim = animator;
    }
    public override void Enter()
    {
        _camTransform = Camera.main.transform;
        _verticalVelocity = _minSpeed;
    }
        
    public override void UpdateState()
    {
        var dir = _stateVars.moveVector;

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
        _verticalVelocity = Mathf.Max(_verticalVelocity - _deceleration * Time.fixedDeltaTime, 0);
        _moveVelocity = _moveDir * _speed;
        _lookDir = _moveDir.normalized;
        
        var moveVector = Vector3.up * _verticalVelocity + _moveVelocity;
        if(_moveDir != Vector3.zero)
            _transform.rotation =
                Quaternion.RotateTowards(_transform.rotation, Quaternion.LookRotation(_lookDir), 2);
        _characterController.Move((moveVector) * Time.fixedDeltaTime);
        if(_verticalVelocity <= 0)
            OnReachedZero?.Invoke(this, EventArgs.Empty);
    }

    public override void Exit()
    {
        
    }
}
