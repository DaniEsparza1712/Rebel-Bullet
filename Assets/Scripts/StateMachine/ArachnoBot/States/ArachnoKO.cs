using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArachnoKO : State
{
    private ArachnoBotMachine _sm;
    private float _koTime;
    private float _timer;

    public ArachnoKO(ArachnoBotMachine am) : base(am)
    {
        _sm = am;
    }

    public override void Enter()
    {
        _koTime = 3;
        _timer = 0;
        _sm.Animator.ResetTrigger("Idle");
        _sm.Animator.ResetTrigger("Attack");
        _sm.Animator.SetTrigger("KO");
        
        Debug.Log("KO");
    }

    public override void UpdateLogic()
    {
        _timer += Time.deltaTime;
        if (_timer >= _koTime)
        {
            _sm.changeTo = "";
            _sm.Animator.SetTrigger("Idle");
            _sm.ChangeState(_sm.idle);
        }
    }

    public override void FixedUpdateLogic()
    {
        
    }
}
