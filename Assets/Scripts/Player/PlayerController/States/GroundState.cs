using DG.Tweening;
using UnityEngine;

public class GroundState : StateBase
{
    private StateVars _stateVars;
    private Vector3 _moveDir;
    private float _speed;
    private Transform _transform;
    private Transform _camTransform;
    private CharacterController _cc;
    
    public GroundState(Animator animator, float speed, StateVars movement, CharacterController cc) : base(animator)
    {
        _speed = speed;
        
        _transform = animator.transform;
        _stateVars = movement;
        _cc = cc;
        
        Anim = animator;
    }
    public override void Enter()
    {
        _stateVars.jumpCount = 0;
        
        if((_stateVars.prevState.GetType() == typeof(Fall) || _stateVars.prevState.GetType() == typeof(JumpEnded) || _stateVars.prevState.GetType() == typeof(Jump)) && _stateVars.moveVector.magnitude < 0.1f)
            Anim.CrossFadeInFixedTime("Land", 0.1f);
        else
            Anim.CrossFadeInFixedTime("Ground", 0.1f);
        Anim.CrossFadeInFixedTime("FE_Neutral", 0.1f);
        _camTransform = Camera.main.transform;
    }
    
    public override void UpdateState()
    {
        var dir = _stateVars.moveVector;

        var walk = Anim.GetFloat("Walk");

        
        DOTween.To(() => walk, x => walk = x, dir.magnitude, 0.1f)
            .OnUpdate(() =>
            {
                Anim.SetFloat("Walk", walk);
            });
        
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
        _cc.Move(_moveDir.normalized * (_speed * Time.fixedDeltaTime) + Physics.gravity * Time.fixedDeltaTime);
        
        if (_moveDir.magnitude > 0 && !_stateVars.gunOut)
            _transform.rotation =
                Quaternion.RotateTowards(_transform.rotation, Quaternion.LookRotation(_moveDir), 25);
        else if (_stateVars.gunOut)
        {
            var rot = _transform.eulerAngles;
            rot.y = _camTransform.eulerAngles.y;
            _transform.rotation = Quaternion.RotateTowards(_transform.rotation, Quaternion.Euler(rot), 25);
            //_transform.rotation = Quaternion.Euler(rot);
        }
    }

    public override void Exit()
    {
        
    }
}
