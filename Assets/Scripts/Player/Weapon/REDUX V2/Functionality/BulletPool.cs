using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BulletPool: MonoBehaviour
{
    private List<BulletController> _bulletPool = new();
    private BulletController _bulletPrefab;

    public void Setup(BulletController bulletPrefab)
    {
        _bulletPrefab = bulletPrefab;
    }
    public void FillPool(int poolSize)
    {
        _bulletPool.Clear();
        for (var i = 0; i < poolSize; i++)
        {
            var bullet = Instantiate(_bulletPrefab, transform);
            bullet.gameObject.SetActive(false);
            bullet.OnShot += (sender, args) => {transform.parent = null;};
            bullet.OnDisappear += (sender, args) => {transform.parent = transform;};
            _bulletPool.Add(bullet);
        }
    }

    public void Shoot(BulletArgs args)
    {
        var bullet = GetFromPool();
        bullet.OnShot(this, args);
        bullet.gameObject.SetActive(true);
    }
    
    private BulletController GetFromPool()
    {
        var instBullet = _bulletPool.FirstOrDefault(bullet => !bullet.gameObject.activeSelf);
        if(instBullet)
            return instBullet;
        
        var newBullet = Instantiate(_bulletPrefab, transform);
        newBullet.gameObject.SetActive(false);
        _bulletPool.Add(newBullet);
        return newBullet;
    }
}
