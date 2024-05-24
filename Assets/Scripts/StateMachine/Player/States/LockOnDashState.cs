using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LockOnDashState : State
{
    private PlayerMachine _sm;
    private Vector3 _directionB;
    private float _hAxisB;
    private float _vAxisB;
    private Vector3 _viewDir;
    private float _timer;
    private float _newXRotation;
    private float _rotationMod;
    private LifeSystem _life;

    public LockOnDashState(PlayerMachine pm) : base(pm)
    {
        _sm = pm;
        _life = pm.GetComponent<LifeSystem>();
    }

    public override void Enter()
    {
        _rotationMod = _sm.lockOnSystem.rotationMod;
        _life.SetInvincible(true);
        _sm.trailObject.SetActive(true);
        
        var hAXis = Input.GetAxisRaw("Horizontal");
        var vAxis = Input.GetAxisRaw("Vertical");
        
        var direction = new Vector3(hAXis, 0, vAxis);
        if(direction.magnitude == 0)
            direction = Vector3.forward;
        
        var viewAngle = -Camera.main.transform.eulerAngles.y;
        _viewDir = MathUtilities.TurnVector(direction, viewAngle);
        _timer = 0;
        
        _sm.animator.speed = 2;
        _sm.animator.SetFloat("Lock On X", hAXis);
        _sm.animator.SetFloat("Lock On Y", vAxis);
        if(hAXis == 0 && vAxis == 0)
            _sm.animator.SetFloat("Lock On Y", 1);
    }

    public override void UpdateLogic()
    {
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
        
        _timer += Time.deltaTime * Time.timeScale;
        if (_timer >= _sm.dashTime)
        {
            _life.SetInvincible(false);
            _sm.trailObject.SetActive(false);
            _sm.animator.speed = 1;
            if (_sm.OnGround())
            {
                _sm.ChangeState(_sm.lockOnBase);
                _sm.animator.ResetTrigger("LockOn");
            }
            else if(!_sm.OnGround())
                _sm.ChangeState(_sm.lockOnFall);
        }
    }

    public override void FixedUpdateLogic()
    {
        _sm.character.Move(_viewDir * _sm.dashSpeed);
        
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
