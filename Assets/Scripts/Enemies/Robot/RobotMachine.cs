using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(NavMeshObstacle))]
[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(EnemyHittable))]
[RequireComponent(typeof(EnemyBulletShooter))]
[RequireComponent(typeof(RagdollManager))]
[RequireComponent(typeof(EnemyNotifier))]
[RequireComponent(typeof(SquadManager))]

public class RoboVars
{
    public Vector3 chasePos;
    public bool patrolling = true;
    public bool notified = false;
}

public class RobotMachine : EnemyStatesMachine
{
    private EnemyHittable _hittable;
    private NavMeshObstacle _obstacle;
    private EnemyBulletShooter _bulletShooter;
    private CapsuleCollider _capsuleCollider;
    private RoboVars _roboVars = new RoboVars();
    private RagdollManager _ragdollManager;
    private EnemyNotifier _notifier;
    
    [Header("Squad")]
    [SerializeField] private SquadManager squad;
    
    [Header("Move data")]
    [SerializeField] private float moveSpeed = 5;
    [SerializeField] private float runSpeed = 8;
    [SerializeField] private List<Transform> waypoints;
    [SerializeField] private float waitTimeMin;
    [SerializeField] private float waitTimeMax;
    
    [Header("Detection Data")]
    [SerializeField] private float detectionAngle = 45f;
    [SerializeField] private float detectionDistance = 3.5f;
    [SerializeField] private Transform target;
    [SerializeField] private Transform eyesPoint;
    [SerializeField] private float stoppingDistance;
    [SerializeField] private float attackRangeDistance;
    [SerializeField] private LayerMask detectionLayer;
    [SerializeField] private LayerMask obstacleLayer;

    [Header("ShootData")] 
    [SerializeField] private float shootTime = 0.5f;

    [Header("AimData")] 
    [SerializeField] private float aimRotOffset = -15;
    [SerializeField] private float minAimTime = 1.5f;
    [SerializeField] private float maxAimTime = 2.5f;
    
    private PatrolState _patrolState;
    private WaitState _waitState;
    private ChaseState _investigateState;
    private AttackWalk _positionForAim;
    private AttackAim _aimState;
    private AttackShoot _shootState;
    private EnemyDeath _deathState;
    private RoboRagdoll _ragdollPatrolling;
    private RoboRagdoll _ragdollAttacking;
    
    private Coroutine _coroutine;

    private void Awake()
    {
        base.Awake();
        
        navMeshAgent = GetComponent<NavMeshAgent>();
        _obstacle = GetComponent<NavMeshObstacle>();
        _hittable = GetComponent<EnemyHittable>();
        _animator = GetComponent<Animator>();
        _capsuleCollider = GetComponent<CapsuleCollider>();
        _ragdollManager = GetComponent<RagdollManager>();
        _ragdollManager = GetComponent<RagdollManager>();
        _notifier = GetComponent<EnemyNotifier>();
        
        target = GameObject.FindGameObjectsWithTag("Player")[0].transform;
        
        CreateStates();

        _initialState = _patrolState;
        _currentState = _patrolState;
        
        SetStatesTransitions();
    }

    private void Update()
    {
        base.Update();
        if (!_roboVars.patrolling)
            return;
        if(FoundTarget())
            OnFoundPlayer?.Invoke(this, EventArgs.Empty);
    }

    public void StartShoot()
    {
        OnStartShoot?.Invoke(this, EventArgs.Empty);
    }

    private bool FoundTarget()
    {
        var targetPos = target.position;
        targetPos.y = transform.position.y;
        var dir = targetPos - transform.position;
        bool onRange = (Vector3.Angle(transform.forward, dir) < detectionAngle) && 
               (Vector3.Distance(transform.position, targetPos) < detectionDistance);
        
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

    private void OnDrawGizmos()
    {
        if (Application.isPlaying)
        {
            var targetPos = target.position;
            targetPos.y = target.position.y + 0.8f;
            var castDir = targetPos - eyesPoint.position;
            Gizmos.color = Color.green;
            Gizmos.DrawRay(eyesPoint.position, castDir);
        }
    }

    private void CreateStates()
    {
        //Patrol States
        _patrolState = new PatrolState(_animator, navMeshAgent, waypoints, moveSpeed);
        _waitState = new WaitState(_animator, navMeshAgent, _obstacle);
        _investigateState = new ChaseState(_animator, navMeshAgent, _roboVars, runSpeed);
        //Aim States
        _positionForAim = new AttackWalk(_animator, navMeshAgent, runSpeed, squad);
        _aimState = new AttackAim(_animator, navMeshAgent, _obstacle, target, attackRangeDistance, aimRotOffset);
        _shootState = new AttackShoot(_animator, navMeshAgent);
        _deathState = new EnemyDeath(_animator, navMeshAgent);
        //Ragdoll States
        _ragdollPatrolling = new RoboRagdoll(_animator, navMeshAgent, _ragdollManager, _roboVars);
        _ragdollAttacking = new RoboRagdoll(_animator, navMeshAgent, _ragdollManager, _roboVars);
    }

    private void SetStatesTransitions()
    {
        _patrolState.OnReachDestination += (sender, args) =>
        {
            ChangeState(_waitState);
            if(_coroutine != null)
                StopCoroutine(_coroutine);
            var waitTime = Random.Range(waitTimeMin, waitTimeMax);
            _coroutine = StartCoroutine(WaitForChangeState(_patrolState, waitTime));
        };
        _investigateState.OnReachDestination += (sender, args) =>
        {
            _roboVars.notified = false;
            ChangeState(_waitState);
            if(_coroutine != null)
                StopCoroutine(_coroutine);
            var waitTime = Random.Range(waitTimeMin, waitTimeMax);
            _coroutine = StartCoroutine(WaitForChangeState(_patrolState, waitTime));
        };
        _positionForAim.OnReachedTarget += (sender, args) =>
        {
            OnStartAim?.Invoke(this, EventArgs.Empty);
            ChangeState(_aimState);
            if(_coroutine != null)
                StopCoroutine(_coroutine);
            _coroutine = StartCoroutine(WaitForAimEnd());
        };
        _aimState.OnRangePassed += (sender, args) =>
        {
            if(_coroutine != null)
                StopCoroutine(_coroutine);
            ChangeState(_positionForAim);
        };
        _aimState.OnStopAiming += (sender, args) =>
        {
            OnEndAim?.Invoke(this, EventArgs.Empty);
        };
        _shootState.OnStopShooting += (sender, args) =>
        {
            OnEndShoot?.Invoke(this, EventArgs.Empty);
            squad.RemoveShooter();
        };
        OnFoundPlayer += (sender, args) =>
        {
            if(_coroutine != null)
                StopCoroutine(_coroutine);
            if (_currentState == _deathState)
                return;
            _roboVars.patrolling = false;
            if (_currentState == _patrolState || _currentState == _waitState || _currentState == _investigateState)
                ChangeState(_positionForAim);
        };
        _hittable.OnHit += (sender, args) =>
        {
            if (_currentState == _deathState)
                return;
            
            _roboVars.chasePos = args.hitOrigin;
            var attack = args.attack;
            switch (attack.element)
            {
                case AttackElement.Concussive:
                    if(_coroutine != null)
                        StopCoroutine(_coroutine);
                    ChangeState(_roboVars.patrolling ? _ragdollPatrolling : _ragdollAttacking);
                    _ragdollManager.ApplyForceAtPoint(attack.elementPoints, args.hitDirection, args.hitPoint);
                    break;
                default:
                    if (_roboVars.patrolling)
                    {
                        if(_coroutine != null)
                            StopCoroutine(_coroutine);
                        ChangeState(_investigateState);
                    }
                    break;
            }
        };
        _hittable.OnDeath += (sender, args) =>
        {
            StopAllCoroutines();
            ChangeState(_deathState);
            OnDeath?.Invoke(this, args);
            enabled = false;
        };
        _notifier.OnNotifiedPos += (sender, args) =>
        {
            if(!_roboVars.patrolling || _roboVars.notified)
                return;
            _roboVars.notified = true;
            if (_coroutine != null)
                StopCoroutine(_coroutine);
            _roboVars.chasePos = args.position;
            ChangeState(_waitState);
            _coroutine = StartCoroutine(WaitForChangeState(_investigateState, 0.4f));
        };
        _ragdollPatrolling.OnHipsSpeedZero += (sender, args) =>
        {
            ChangeState(_investigateState);
        };
        _ragdollAttacking.OnHipsSpeedZero += (sender, args) =>
        {
            ChangeState(_positionForAim);
        };
    }

    private bool HasClearShot()
    {
        var castPos = transform.position + Vector3.up * 1.2f;
        var dir = target.position - castPos;
        return !Physics.Raycast(castPos, dir.normalized, out _, detectionDistance, obstacleLayer);
    }

    private IEnumerator WaitForAimEnd()
    {
        yield return new WaitForSeconds(Random.Range(minAimTime, maxAimTime));
        if (HasClearShot() && squad.CanShoot)
        {
            ChangeState(_shootState);
            OnStartShoot?.Invoke(this, EventArgs.Empty);
            squad.AddShooter();
            StartCoroutine(WaitForChangeState(_positionForAim, shootTime));
        }
        else if(!HasClearShot())
            ChangeState(_positionForAim);
    }
}
