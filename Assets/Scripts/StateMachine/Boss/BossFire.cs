using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossFire : State
{
    private BossMachine _sm;
    private Vector3 _lookVector;
    private Vector3 _newLook;
    private Vector3 _playerPos;
    public BossFire(BossMachine bm) : base(bm)
    {
        _sm = bm;
    }

    public override void Enter()
    {
        _sm.Animator.SetTrigger("Fire");
    }
    public override void UpdateLogic()
    {
        if(_sm.changeTo == "Death")
        {
            _sm.ChangeState(_sm.Death);
        }
        else if (_sm.changeTo == "Idle")
        {
            _sm.ChangeState(_sm.Idle);
            if(_sm.currentState != this)
                _sm.ChangeTo("");
        }
    }
    public override void FixedUpdateLogic()
    {
        _playerPos = _sm.playerTransform.position;
        _playerPos.y = _sm.transform.position.y;
        _lookVector = (_playerPos - _sm.transform.position).normalized;
        _newLook = Vector3.RotateTowards(_sm.transform.forward, _lookVector, 30, 0.0f);
        _sm.transform.rotation = Quaternion.LookRotation(_newLook);
    }
}
