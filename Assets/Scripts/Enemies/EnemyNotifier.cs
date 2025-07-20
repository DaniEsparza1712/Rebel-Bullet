using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyNotifier : MonoBehaviour
{
    private EnemyStatesMachine _stateMachine;
    private EnemyHittable _hittable;
    [SerializeField] private float notifyRadius;
    public EventHandler<NotifyArgs> OnNotifiedPos;
    private Transform _playerTransform;

    void Awake()
    {
        _playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        _hittable = GetComponent<EnemyHittable>();
        _stateMachine = GetComponent<EnemyStatesMachine>();
        _hittable.OnHit += (sender, args) => { NotifyPos(args.hitOrigin); };
        _stateMachine.OnFoundPlayer += (sender, args) => { NotifyPos(_playerTransform.position); };
    }

    private void NotifyPos(Vector3 hitPosition)
    {
        var notified = Physics.OverlapSphere(transform.position, notifyRadius, LayerMask.GetMask("Enemy"));
        foreach (var enemy in notified)
        {
            if (enemy.gameObject == gameObject)
                continue;
            if (enemy.gameObject.TryGetComponent<EnemyNotifier>(out var notifier))
            {
                notifier.OnNotifiedPos?.Invoke(this, new NotifyArgs(hitPosition));
            }
        }
    }
}
