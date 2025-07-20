using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(EnemyBulletShooter))]
[RequireComponent(typeof(EnemyHittable))]
public class TurretController : EnemyStatesMachine
{
    [SerializeField]
    private Transform target;
    
    private EnemyBulletShooter _bulletShooter;
    private EnemyHittable _hittable;

    [Header("Detection Data")]
    [SerializeField] private Transform eyesPoint;
    [SerializeField] private float detectionDistance;
    [SerializeField] private float detectionAngle;
    [SerializeField] private LayerMask detectionLayer;
    
    [Header("Squad")]
    [SerializeField] private SquadManager squad;
    
    [Header("Aiming")]
    [SerializeField] private float minAimTime;
    [SerializeField] private float maxAimTime;
    
    [Header("Shooting")]
    [SerializeField] private float shootTime;

    private EmptyState _searchState;
    private EmptyState _aimState;
    private EmptyState _shootState;

    private void Awake()
    {
        base.Awake();
        
        _bulletShooter = GetComponent<EnemyBulletShooter>();
        _hittable = GetComponent<EnemyHittable>();
        
        CreateStates();
        SetStateTransitions();

        _initialState = _searchState;
        _currentState = _searchState;
    }

    private void Update()
    {
        base.Update();
        
        if (_currentState != _searchState)
            return;
        if (FoundTarget())
            OnFoundPlayer?.Invoke(this, EventArgs.Empty);
            
    }

    private void CreateStates()
    {
        _searchState = new EmptyState(_animator, "Search");
        _aimState = new EmptyState(_animator, "Empty");
        _shootState = new EmptyState(_animator, "Attack");
    }

    private void SetStateTransitions()
    {
        OnFoundPlayer += (sender, args) =>
        {
            OnStartAim?.Invoke(this, args);
            ChangeState(_aimState);
            
            StopAllCoroutines();
            StartCoroutine(WaitForAimEnd());
        };
        OnEndShoot += (sender, args) =>
        {
            squad.RemoveShooter();
            
            OnStartAim?.Invoke(this, args);
            ChangeState(_aimState);
            
            StopAllCoroutines();
            StartCoroutine(WaitForAimEnd());
        };
        _hittable.OnDeath += (sender, args) =>
        {
            StopAllCoroutines();
            _animator.CrossFadeInFixedTime("Death", 0.1f);
            OnDeath?.Invoke(this, args);
            enabled = false;
        };
    }
    
    private IEnumerator WaitForAimEnd()
    {
        yield return new WaitForSeconds(Random.Range(minAimTime, maxAimTime));
        if (!_bulletShooter.HasClearShot() && !squad.CanShoot)
            yield break;
        
        ChangeState(_shootState);
        OnEndAim?.Invoke(this, EventArgs.Empty);
        OnStartShoot?.Invoke(this, EventArgs.Empty);
        squad.AddShooter();
        yield return new WaitForSeconds(shootTime);
        OnEndShoot?.Invoke(this, EventArgs.Empty);
    }
    
    private bool FoundTarget()
    {
        var targetPos = target.position;
        targetPos.y = eyesPoint.position.y;
        var dir = targetPos - eyesPoint.position;
        bool onRange = (Vector3.Angle(eyesPoint.forward, dir) < detectionAngle) && 
                       (Vector3.Distance(eyesPoint.position, targetPos) < detectionDistance);
        
        if(!onRange)
            return false;
        targetPos.y = target.position.y + 0.8f;
        var castDir = targetPos - eyesPoint.position;
        if (Physics.Raycast(eyesPoint.position, castDir, out var hit, detectionDistance, detectionLayer))
        {
            if (hit.transform.gameObject.CompareTag("Player"))
                return true;
        }
        return false;
    }
    
}
