using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StandardStyle : ChipStyle
{
    [Header("Bullets")]
    [SerializeField]
    private BulletController bullet;
    private List<BulletController> _bullets = new List<BulletController>();
    [SerializeField] private int poolSize = 7;
    
    [Header("Positions")]
    [SerializeField] private Transform bulletSpawn;
    [SerializeField] private Transform aimPoint;
    [SerializeField] private float shootRate;

    private bool _pressingShoot;

    private void Awake()
    {
        FillPool();
    }

    private void FillPool()
    {
        _bullets.Clear();
        for (var i = 0; i < poolSize; i++)
        {
            var instBullet = Instantiate(bullet, transform);
            instBullet.gameObject.SetActive(false);
            _bullets.Add(instBullet);
        }
    }
    
    private void Shoot(BulletController instBullet)
    {
        var dir = aimPoint.position - bulletSpawn.position;
            
        //instBullet.OnShot?.Invoke(this, new BulletArgs(dir));
        instBullet.transform.parent = null;
        instBullet.transform.position = bulletSpawn.position;
        instBullet.gameObject.SetActive(true);
    }

    private BulletController GetFromPool()
    {
        foreach (var instBullet in _bullets)
        {
            if(!instBullet.gameObject.activeSelf)
                return instBullet;
        }
        var newBullet = Instantiate(bullet, transform);
        newBullet.gameObject.SetActive(false);
        _bullets.Add(newBullet);
        return newBullet;
    }
    
    private IEnumerator ShootCoroutine()
    {
        while (_pressingShoot)
        {
            Shoot(GetFromPool());
            
            yield return new WaitForSeconds(shootRate);
        }
    }
    
    public override void StartShooting()
    {
        _pressingShoot = true;
        StartCoroutine(ShootCoroutine());
    }

    public override void StopShooting()
    {
        _pressingShoot = false;
    }
}
