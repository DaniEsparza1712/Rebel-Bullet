using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BossMachine : StateMachine
{
    [HideInInspector]public string changeTo;
    [HideInInspector] public BossIdle Idle;
    [HideInInspector] public BossFire Fire;
    [HideInInspector] public BossFloor Floor;
    [HideInInspector] public BossRegenerate Regenerate;
    [HideInInspector] public BossDeath Death;
    
    [HideInInspector] public Transform playerTransform;

    [Header("External Components")] public Animator Animator;
    public LifeSystem lifeSystem;
    [Header("Life")] public float regenerationRate;
    public int regenerationValue;

    private void Awake()
    {
        Idle = new BossIdle(this);
        Fire = new BossFire(this);
        Floor = new BossFloor(this);
        Regenerate = new BossRegenerate(this);
        Death = new BossDeath(this);
        
        currentState = Idle;
        playerTransform = GameObject.Find("Sam").GetComponent<Transform>();
    }

    public void ChangeTo(string change)
    {
        changeTo = change;
    }

    public void DeathEvent()
    {
        ChangeTo("Death");
    }
}
