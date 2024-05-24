using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Bullet : MonoBehaviour
{
    public string targetTag;
    public Rigidbody rb;
    public GameObject hitFlash;
    public bool destroyable;
    private float _lifetime;
    private float _speed;
    private int _damage;
    public bool simulatePhysics;
    public UnityEvent enemyHitEvent;
    private bool _hasBeenEnabled = false;
    private void Start()
    {
        _lifetime = ComponentHolder.Instance.BulletLife;
        _damage = ComponentHolder.Instance.Damage;
        _speed = ComponentHolder.Instance.BulletSpeed;
        
        transform.localScale = Vector3.one * ComponentHolder.Instance.BulletSize;
        StartCoroutine(LifeCoroutine());
        if (simulatePhysics)
        {
            rb.useGravity = true;
            rb.AddForce(transform.up + transform.forward * _speed, ForceMode.Impulse);
        }
    }

    private void OnEnable()
    {
        if (_hasBeenEnabled)
        {
            _lifetime = ComponentHolder.Instance.BulletLife;
            _damage = ComponentHolder.Instance.Damage;
            _speed = ComponentHolder.Instance.BulletSpeed;
        
            transform.localScale = Vector3.one * ComponentHolder.Instance.BulletSize;
            StartCoroutine(LifeCoroutine());
            if (simulatePhysics)
            {
                rb.useGravity = true;
                rb.AddForce(transform.up + transform.forward * _speed, ForceMode.Impulse);
            }
            GetComponentInChildren<TrailRenderer>().Clear();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(!simulatePhysics)
            rb.velocity = transform.forward * _speed;
    }
    private void OnTriggerEnter(Collider other) {
        if (other.CompareTag(targetTag))
        {
            LifeSystem enemyLife = other.GetComponent<LifeSystem>();
            enemyLife.ApplyDamage(_damage);
            enemyHitEvent.Invoke();
        }
        if(gameObject.CompareTag("PlayerBullet") && other.CompareTag("EnemyBullet"))
        {
            if(other.gameObject.GetComponent<EnemyBullet>().destroyable)
                other.gameObject.SetActive(false);
            Instantiate(hitFlash, transform.position, Quaternion.identity);
            gameObject.SetActive(false);
        }
        if(other.CompareTag(targetTag) || other.CompareTag("Wall") || other.CompareTag("Floor") || other.CompareTag("Explodable")){
            Instantiate(hitFlash, transform.position, Quaternion.identity);
            gameObject.SetActive(false);
        }
    }
    private void OnDisable()
    {
        if(!_hasBeenEnabled)
            _hasBeenEnabled = true;
    }
    
    private IEnumerator LifeCoroutine()
    {
        yield return new WaitForSeconds(_lifetime);
        Instantiate(hitFlash, transform.position, Quaternion.identity);
        gameObject.SetActive(false);
    }
}
