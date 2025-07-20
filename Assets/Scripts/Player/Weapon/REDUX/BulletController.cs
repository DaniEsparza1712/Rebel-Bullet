using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour
{
    [SerializeField] private float _speed;
    [SerializeField] private float lifetime;
    [SerializeField] private Attack attack;
    private Rigidbody _rb;
    private Vector3 _fwd;
    [SerializeField] private List<String> hitTags;
    
    [Header("Visuals")]
    [SerializeField] private GameObject hit;
    [SerializeField] protected ParticleSystem hitPS;
    [SerializeField] protected GameObject flash;
    [SerializeField] protected ParticleSystem projectilePS;
    [SerializeField] protected GameObject[] Detached;
    [SerializeField] protected Light light;
    
    public EventHandler<BulletArgs> OnShot;
    public EventHandler OnImpact;
    public EventHandler OnDisappear;
    private Vector3 _startPos;
    private bool _moving = true;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
        _rb.useGravity = false;
        _rb.constraints = RigidbodyConstraints.FreezeRotation;
        OnShot += (sender, args) =>
        {
            if(light)
                light.enabled = true;
            
            //Set Args
            _fwd = args.Dir;
            _speed = args.Speed;
            attack = args.BulletAttack;
            transform.position = args.Pos;
            
            transform.forward = _fwd;
            _moving = true;
        };
        OnImpact += (sender, args) =>
        {
        };
        OnDisappear += (sender, args) =>
        {
            gameObject.SetActive(false);
        };
    }

    private void OnEnable()
    {
        _rb.constraints = RigidbodyConstraints.FreezeRotation;
        _startPos = transform.position;
        StopAllCoroutines();
        StartCoroutine(BulletLifetime());
    }

    private void FixedUpdate()
    {
        if(_moving)
            _rb.velocity = _fwd * _speed;
    }

    private IEnumerator BulletLifetime()
    {
        yield return new WaitForSeconds(lifetime);
        gameObject.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!hitTags.Contains(other.tag))
            return;
        _moving = false;
        _rb.velocity = Vector3.zero;
        StopAllCoroutines();
        _rb.constraints = RigidbodyConstraints.FreezePosition;
        
        if(light)
            light.enabled = false;
        
        if (projectilePS)
        {
            projectilePS.Stop();
            projectilePS.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
        }
        
        foreach (var detachedPrefab in Detached)
        {
            if (detachedPrefab != null)
            {
                ParticleSystem detachedPS = detachedPrefab.GetComponent<ParticleSystem>();
                detachedPS.Stop();
            }
        }
        
        if (other.TryGetComponent<EnemyHittable>(out var hittable))
        {
            var hitPoint = other.ClosestPoint(transform.position);
            var dir = transform.forward + Vector3.up * 0.5f;
            var args = new AttackArgs(attack, hitPoint, dir.normalized, _startPos);
            hittable.OnGetHit(args);
        }
        OnImpact?.Invoke(this, EventArgs.Empty);
        
        SpawnHit(other);
    }

    private void SpawnHit(Collider hitCollider)
    {
        var pos = hitCollider.ClosestPoint(transform.position);
        var lookRot = -transform.forward;
        
        hit.transform.position = pos;
        hit.transform.forward = lookRot;
        hitPS.Play();
    }
}
