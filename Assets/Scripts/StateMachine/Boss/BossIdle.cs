using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossIdle : State
{
    private float _waitTime;
    private float _minWait = 1.5f;
    private float _maxWait = 2.5f;
    private float _timer;
    private float _stateNum;
    private BossMachine _sm;
    public BossIdle(BossMachine bm) : base(bm)
    {
        _sm = bm;
    }

    public override void Enter()
    {
        _stateNum = Random.Range(0, 3);
        _sm.Animator.SetTrigger("Idle");
        _waitTime = Random.Range(_minWait, _maxWait);
        _timer = 0;
    }
    public override void UpdateLogic()
    {
        _timer += Time.deltaTime;
        if(_sm.changeTo == "Death")
        {
            _sm.ChangeState(_sm.Death);
        }
        else if (_timer >= _waitTime)
        {
            switch (_stateNum)
            {
                case 0:
                    _sm.ChangeState(_sm.Regenerate);
                    break;
                case 1:
                    _sm.ChangeState(_sm.Fire);
                    break;
                case 2:
                    _sm.ChangeState(_sm.Floor);
                    break;
            }
        }
    }
    public override void FixedUpdateLogic()
    {
        
    }
}
