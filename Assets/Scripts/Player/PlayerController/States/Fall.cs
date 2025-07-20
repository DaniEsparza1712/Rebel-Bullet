using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fall : StateBase
{
    private StateVars _stateVars;
    private Vector3 _moveDir;
    private Vector3 _moveVelocity;
    private Vector3 _lookDir;
    
    private float _speed;
    private Transform _transform;
    private Transform _camTransform;
    private Rigidbody _rb;
    private CapsuleCollider _capsuleCollider;
    private CharacterController _characterController;
    private float _rotationMod = 50;
    private float _terminalVelocity;
    private float _acceleration;
    private float _verticalVelocity;
        
    public Fall(Animator animator, float speed, float acceleration, float terminal, StateVars movement, CharacterController controller) : base(animator)
    {
        _speed = speed;
        _transform = animator.transform;
        _stateVars = movement;
        _characterController = controller;

        _terminalVelocity = terminal;
        _acceleration = acceleration;
            
        Anim = animator;
    }
    public override void Enter()
    {
        Anim.CrossFadeInFixedTime("Fall", 0.1f);

        _verticalVelocity = 0;
        _camTransform = Camera.main.transform;
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
        _verticalVelocity = Mathf.Max(_verticalVelocity + _acceleration * Time.fixedDeltaTime, Mathf.Abs(_terminalVelocity));
        _moveVelocity = _moveDir * _speed;
        _lookDir = _moveDir.normalized;
        
        var moveVector = Vector3.up * (-_verticalVelocity) + _moveVelocity;
        if(_moveDir != Vector3.zero)
            _transform.rotation =
                Quaternion.RotateTowards(_transform.rotation, Quaternion.LookRotation(_lookDir), 2);
        _characterController.Move((moveVector) * Time.fixedDeltaTime);
    }

    public override void Exit()
    {
        
    }
}
