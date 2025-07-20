using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class EnemyRigController : MonoBehaviour
{
    private EnemyStatesMachine _stateMachine;
    [SerializeField] private Rig rig;

    private void Awake()
    {
        _stateMachine = GetComponent<EnemyStatesMachine>();

        _stateMachine.OnStartAim += (sender, args) =>
        {
            rig.weight = 1;
        };
        _stateMachine.OnEndAim += (sender, args) =>
        {
            rig.weight = 0;
        };
        _stateMachine.OnStartShoot += (sender, args) =>
        {
            rig.weight = 1;
        };
        _stateMachine.OnDeath += (sender, args) =>
        {
            rig.weight = 0;
        };
    }
}
