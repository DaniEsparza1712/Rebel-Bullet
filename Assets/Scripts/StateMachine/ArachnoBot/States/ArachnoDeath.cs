using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArachnoDeath : State
{
    private ArachnoBotMachine _sm;

    public ArachnoDeath(ArachnoBotMachine am) : base(am)
    {
        _sm = am;
    }

    public override void Enter()
    {
        
    }

    public override void UpdateLogic()
    {

    }

    public override void FixedUpdateLogic()
    {
        
    }
}
