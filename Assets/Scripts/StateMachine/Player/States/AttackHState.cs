using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackHState : State
{
    private PlayerMachine _sm;
    Vector3 direction;
    float viewAngle;
    Vector3 viewDir;
    float hAxis;
    float vAxis;
    float rotationMod = 100;
    private int _attackCounter;

    public AttackHState(PlayerMachine pm) : base(pm)
    {
        _sm = pm;
    }

    public override void Enter()
    {
        _attackCounter = 1;
        _sm.animator.ResetTrigger("Base");
        _sm.animator.SetInteger("AttackH", _attackCounter);
        _sm.animator.SetTrigger("FE_Joy");
    }

    public override void UpdateLogic()
    {
        hAxis = Input.GetAxis("Horizontal");
        vAxis = Input.GetAxis("Vertical");
        direction = new Vector3(hAxis, 0, vAxis);

        viewAngle = -Camera.main.transform.eulerAngles.y;

        viewDir = MathUtilities.TurnVector(direction, viewAngle);
        if (_sm.changeTo == "idle")
        {
            _sm.ChangeState(_sm.baseState);
            if(_sm.currentState != this)
                _sm.changeTo = "";
        }
    }

    public override void FixedUpdateLogic()
    {
        if(viewDir.magnitude > 0 + Mathf.Epsilon)
            _sm.transform.rotation = Quaternion.RotateTowards(_sm.transform.rotation, Quaternion.LookRotation(viewDir), _sm.speed * rotationMod);
    }
}
