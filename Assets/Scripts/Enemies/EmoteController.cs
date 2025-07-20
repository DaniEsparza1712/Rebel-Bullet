using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EmoteController : MonoBehaviour
{
    private EnemyStatesMachine _stateMachine;
    [SerializeField] private GameObject enemyEmote;
    [SerializeField] private GameObject enemyActiveEmote;
    [SerializeField] private GameObject surpriseQuestionEmote;
    [SerializeField] private GameObject surpriseEmote;
    [SerializeField] private GameObject questionEmote;

    private void Awake()
    {
        _stateMachine = GetComponent<EnemyStatesMachine>();
        _stateMachine.OnSurprise += (sender, args) =>
        {
            surpriseEmote.SetActive(true);
            surpriseQuestionEmote.SetActive(false);
            questionEmote.SetActive(false);
        };
        _stateMachine.OnEndSurprise += (sender, args) => { surpriseEmote.SetActive(false); };
        
        _stateMachine.OnStartAim += (sender, args) =>
        {
            enemyEmote.SetActive(false);
            enemyActiveEmote.SetActive(true);
        };
        
        _stateMachine.OnSurpriseQuestion += (sender, args) => { surpriseQuestionEmote.SetActive(true); };
        _stateMachine.OnEndSurpriseQuestion += (sender, args) => { surpriseQuestionEmote.SetActive(false); };
        
        _stateMachine.OnQuestion += (sender, args) => { questionEmote.SetActive(true); };
        _stateMachine.OnEndQuestion += (sender, args) => { questionEmote.SetActive(false); };

        _stateMachine.OnDeath += (sender, args) =>
        {
            enemyEmote.SetActive(false);
            enemyActiveEmote.SetActive(false);
            questionEmote.SetActive(false);
            surpriseEmote.SetActive(false);
            surpriseQuestionEmote.SetActive(false);
        };
        
        enemyEmote.SetActive(true);
    }
}
