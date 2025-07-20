using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dash : StateBase
{
    private StateVars _stateVars;
    private Vector3 _moveDir;
    private float _speed;
    private Transform _transform;
    private Transform _camTransform;
    private CharacterController _characterController;
    private Rigidbody _rigidbody;
    private CapsuleCollider _capsuleCollider;
    
    public Dash(Animator animator, float speed, StateVars movement, CharacterController character, Rigidbody rb, 
        CapsuleCollider collider) : base(animator)
    {
        _speed = speed;
        _transform = animator.transform;
        _stateVars = movement;
        
        _characterController = character;
        _rigidbody = rb;
        _capsuleCollider = collider;
        
        Anim = animator;
    }
    public override void Enter()
    {
        _characterController.enabled = true;
        
        _camTransform = Camera.main.transform;
        var dir = _stateVars.moveVector;

        if (dir == Vector3.zero)
            dir = new Vector3(0, 0, 1);

        var camFwd = _camTransform.forward;
        var camRight = _camTransform.right;
        
        camFwd.y = 0;
        camRight.y = 0;
        camFwd.Normalize();
        camRight.Normalize();
        
        _moveDir = camFwd * dir.z + camRight * dir.x;
        
        Anim.CrossFadeInFixedTime("Dash", 0.1f);
    }
    
    public override void UpdateState()
    {
        
    }
    
    public override void UpdatePhysics()
    {
        _characterController.Move(_moveDir * _speed);
        if (_moveDir.magnitude > 0 && !_stateVars.gunOut)
            _transform.rotation =
                Quaternion.RotateTowards(_transform.rotation, Quaternion.LookRotation(_moveDir), 25);
        else if (_stateVars.gunOut)
        {
            var rot = _transform.eulerAngles;
            rot.y = _camTransform.eulerAngles.y;
            _transform.rotation = Quaternion.RotateTowards(_transform.rotation, Quaternion.Euler(rot), 25);
        }
    }

    public override void Exit()
    {
        
    }
}
