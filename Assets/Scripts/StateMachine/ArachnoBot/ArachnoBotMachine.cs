using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class ArachnoBotMachine : StateMachine
{
    [HideInInspector]
    public ArachnoIdle idle;
    [HideInInspector]
    public ArachnoFollow follow;
    [HideInInspector]
    public ArachnoShooting shooting;
    [HideInInspector]
    public ArachnoDeath death;
    [HideInInspector] 
    public ArachnoKO ko;
    
    [HideInInspector]
    public GameObject target;
    public NavMeshAgent agent;
    public float sight;
    public LayerMask sightMask;

    [Header("Ragdoll")] 
    public LayerMask floorMask;

    [Header("Speed")]
    public float followSpeed;
    public float shootingSpeed;

    public Animator Animator;
    [HideInInspector] 
    public string changeTo;
    private void Awake()
    {
        idle = new ArachnoIdle(this);
        follow = new ArachnoFollow(this);
        shooting = new ArachnoShooting(this);
        death = new ArachnoDeath(this);
        ko = new ArachnoKO(this);
        
        currentState = idle;
        target = GameObject.Find("Sam"); 
        idle.Enter();
    }

    public void BackToIdle()
    {
        ChangeState(idle);
    }

    public void DeathEvent()
    {
        ChangeState(death);
    }

    public void KO()
    {
        changeTo = "KO";
    }
}
