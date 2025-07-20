using UnityEngine;

public abstract class StateBase
{
    protected Animator Anim;
    public StateBase(Animator animator)
    {
    }
    public abstract void Enter();
    public abstract void UpdateState();
    public abstract void UpdatePhysics();
    public abstract void Exit();
}