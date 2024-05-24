using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class LockOnBaseState : State
{
    private PlayerMachine _sm;
    private Vector3 _direction;
    private Vector3 _directionB;
    private float _viewAngle;
    private Vector3 _viewDir;
    private float _hAxis;
    private float _vAxis;
    private float _hAxisB;
    private float _vAxisB;
    private float _scroll;
    private float _newXRotation;
    private float _rotationMod;
    public LockOnBaseState(PlayerMachine pm): base(pm){
        _sm = pm;
    }

    public override void Enter()
    {
        _rotationMod = _sm.lockOnSystem.rotationMod;
        _sm.rb.isKinematic = true;
        _sm.mainCollider.enabled = false;
        _sm.character.enabled = true;
        
        _sm.ik_Controller.pointAt = true;
        _sm.rig.weight = 1;
        _sm.lockOnTr.forward = Camera.main.transform.forward;
        _sm.animator.ResetTrigger("Base");
        _sm.animator.SetTrigger("FE_Angry");

        if(_sm.cameraSwitcher.mainCamera.enabled)
            _sm.ForceFaceForward();

        _sm.cameraSwitcher.SwitchCamera(CameraSwitcher.Cameras.lockOn);
        _newXRotation = _sm.lockOnSystem.currentXRotation;
        _sm.animator.SetTrigger("LockOn");
        _sm.lockOnIcon.SetActive(true);

        _sm.weaponAppear.Appear();
    }
    public override void UpdateLogic() {
        _hAxis = Input.GetAxis("Horizontal");
        _vAxis = Input.GetAxis("Vertical");

        _direction = new Vector3(_hAxis, 0, _vAxis);

        _sm.animator.SetFloat("Lock On X", _hAxis);
        _sm.animator.SetFloat("Lock On Y", _vAxis);

        _viewAngle = -Camera.main.transform.eulerAngles.y;

        _viewDir = MathUtilities.TurnVector(_direction, _viewAngle);

        if (_sm.lockOnSystem.lockMode == LockOnSystem.LockMode.Free) {
            _hAxisB = Input.GetAxis("Horizontal B");
            _vAxisB = Input.GetAxis("Vertical B");

            _directionB = new Vector3(_vAxisB, _hAxisB, 0);
            
            _newXRotation += _directionB.x * _sm.speed * _rotationMod;
            _sm.lockOnSystem.currentXRotation = _newXRotation;
            _newXRotation = Mathf.Clamp(_newXRotation, _sm.lockOnSystem.minXRotation, _sm.lockOnSystem.maxXRotation);
        }
        else {
            _hAxisB = Input.GetAxisRaw("Horizontal B");
        }

        if ((Input.GetAxisRaw("Lock On") <= 0 && Input.GetAxisRaw("Fire1") <= 0) || (Input.GetButtonUp("Lock On") && !Input.GetButton("Fire1"))){
            _sm.transform.rotation = Quaternion.Euler(0, _sm.transform.eulerAngles.y, 0);
            _sm.ChangeState(_sm.baseState);
        }
        else if (Input.GetButtonDown("Jump"))
        {
            _sm.ChangeState(_sm.lockOnJump);
        }
        else if (!_sm.OnGround())
        {
            _sm.ChangeState(_sm.lockOnFall);
        }
        else if (Input.GetButtonDown("Dash"))
        {
            _sm.ChangeState(_sm.LockOnDashState);
        }
    }

    public override void FixedUpdateLogic(){
        _sm.character.Move(_viewDir * _sm.speed + Physics.gravity/50.0f);
        float x = _sm.lockOnTr.transform.localEulerAngles.x + _directionB.x * _sm.speed * _rotationMod;
        float y = _sm.lockOnTr.transform.localEulerAngles.y + _directionB.y * _sm.speed * _rotationMod;
        Vector3 shoulderRot = new Vector3(x, y, 0);
        shoulderRot = Quaternion.Euler(shoulderRot).eulerAngles;
        if (shoulderRot.x < 330 && shoulderRot.x > 300)
            shoulderRot.x = 330;
        else if (shoulderRot.x > 45 && shoulderRot.x < 60)
            shoulderRot.x = 45;
        _sm.lockOnTr.transform.localEulerAngles = shoulderRot;

        Quaternion bodyRot = Quaternion.Euler(0, _sm.lockOnTr.transform.eulerAngles.y, 0);
        _sm.transform.rotation = Quaternion.RotateTowards(_sm.transform.rotation, bodyRot, _rotationMod);
    }
}
