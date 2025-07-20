using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class EnemyStatesMachine : StatesMachine
{
    protected NavMeshAgent navMeshAgent;
    public EventHandler OnStartAim;
    public EventHandler OnEndAim;
    public EventHandler OnStartShoot;
    public EventHandler OnEndShoot;
    public EventHandler OnSurprise;
    public EventHandler OnEndSurprise;
    public EventHandler OnQuestion;
    public EventHandler OnEndQuestion;
    public EventHandler OnSurpriseQuestion;
    public EventHandler OnEndSurpriseQuestion;
    public EventHandler OnFoundPlayer;
    public EventHandler OnDeath;
    
    protected IEnumerator WaitForChangeState(StateBase newState, float time)
    {
        yield return new WaitForSeconds(time);
        ChangeState(newState);
    }
}
