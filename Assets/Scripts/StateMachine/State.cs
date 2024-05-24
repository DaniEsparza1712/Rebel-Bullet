using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class State
{
    public State(StateMachine stateMachine){
        
    }
    public abstract void Enter();
    public abstract void UpdateLogic();
    public abstract void FixedUpdateLogic();
}
