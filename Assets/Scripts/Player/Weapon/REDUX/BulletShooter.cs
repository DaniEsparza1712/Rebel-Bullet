using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletShooter : MonoBehaviour
{
    private bool _isShooting;

    [Header("Positions")] 
    [SerializeField] private Transform spawnPos;
    [SerializeField] private Transform aimPos;
    
    //Stats
    private Attack _currentAttack;
    private float _bulletSpeed;
    private float _shootRate;
    
    //Objects
    private BulletPool _currentPool;

    public void SetStats(Attack attack, float speed, float shootRate)
    {
        _currentAttack = attack;
        _bulletSpeed = speed;
        _shootRate = shootRate;
    }

    public void SetPool(BulletPool pool)
    {
        _currentPool = pool;
    }

    public void StartShooting()
    {
        _isShooting = true;
        StartCoroutine(ShootCoroutine());
    }

    public void StopShooting()
    {
        _isShooting = false;
    }

    private IEnumerator ShootCoroutine()
    {
        while (_isShooting)
        {
            var dir = (aimPos.position - spawnPos.position).normalized;
            var args = new BulletArgs(dir, spawnPos.position, _currentAttack, _bulletSpeed);
            _currentPool.Shoot(args);

            yield return new WaitForSeconds(_shootRate);
        }
    }
}
