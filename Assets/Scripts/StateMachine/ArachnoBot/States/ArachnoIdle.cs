using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArachnoIdle : State
{
    private ArachnoBotMachine _sm;

    public ArachnoIdle(ArachnoBotMachine am) : base(am)
    {
        _sm = am;
    }

    public override void Enter()
    {
        
    }

    public override void UpdateLogic()
    {
        if (_sm.changeTo == "KO")
        {
            _sm.ChangeState(_sm.ko);
        }
        else if (LookForPlayer())
        {
            _sm.ChangeState(_sm.follow);
        }
    }

    public override void FixedUpdateLogic()
    {
        
    }

    private bool LookForPlayer()
    {
        Collider[] foundCols = Physics.OverlapSphere(_sm.transform.position, _sm.sight, _sm.sightMask);
        if(foundCols.Length > 0)
            return true;
        return false;
    }
}
