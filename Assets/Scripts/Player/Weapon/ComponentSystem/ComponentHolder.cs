using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ComponentHolder
{
    public static ComponentHolder Instance;
    
    private BarrelComponent _barrel;
    private GripComponent _grip;
    private MagazineComponent _magazine;
    private MuzzleComponent _muzzle;
    private SpringComponent _spring;

    [HideInInspector]
    public float Accuracy;
    [HideInInspector]
    public float BulletLife;
    [HideInInspector]
    public float BulletSize;
    [HideInInspector]
    public float BulletSpeed;
    [HideInInspector]
    public int Damage;
    [HideInInspector]
    public float ShootingRate;
    [HideInInspector]
    public float OverheatRate;

    public ComponentHolder(BarrelComponent barr, GripComponent grip, MagazineComponent mag, MuzzleComponent muz, SpringComponent spr)
    {
        _barrel = barr;
        _grip = grip;
        _magazine = mag;
        _muzzle = muz;
        _spring = spr;
    }

    public void SetBarrel(BarrelComponent barr)
    {
        _barrel = barr;
        GetStats();
    }

    public void SetGrip(GripComponent grp)
    {
        _grip = grp;
        GetStats();
    }

    public void SetMagazine(MagazineComponent mag)
    {
        _magazine = mag;
        GetStats();
    }

    public void SetMuzzle(MuzzleComponent muz)
    {
        _muzzle = muz;
        GetStats();
    }

    public void SetSprint(SpringComponent spr)
    {
        _spring = spr;
        GetStats();
    }

    public void SetComponents(BarrelComponent barr, GripComponent grip, MagazineComponent mag, MuzzleComponent muz, SpringComponent spr)
    {
        _barrel = barr;
        _grip = grip;
        _magazine = mag;
        _muzzle = muz;
        _spring = spr;
    }
    
    private void GetAccuracy()
    {
        Accuracy = 0;
        Accuracy += _barrel.accuracy;
        Accuracy += _grip.accuracy;
        Accuracy += _magazine.accuracy;
        Accuracy += _muzzle.accuracy;
        Accuracy += _spring.accuracy;

        Accuracy = Mathf.Clamp(Accuracy, 0.1f, Accuracy);
    }

    private void GetBulletLife()
    {
        BulletLife = 0;
        BulletLife += _barrel.accuracy;
        BulletLife += _grip.bulletLife;
        BulletLife += _magazine.bulletLife;
        BulletLife += _muzzle.bulletLife;
        BulletLife += _spring.bulletLife;

        BulletLife = Mathf.Clamp(BulletLife, 0.5f, BulletLife);
    }

    private void GetBulletSize()
    {
        BulletSize = 0;
        BulletSize += _barrel.bulletSize;
        BulletSize += _grip.bulletSize;
        BulletSize += _magazine.bulletSize;
        BulletSize += _muzzle.bulletSize;
        BulletSize += _spring.bulletSize;

        BulletSize = Mathf.Clamp(BulletSize, 0.1f, BulletSize);
    }

    private void GetBulletSpeed()
    {
        BulletSpeed = 0;
        BulletSpeed += _barrel.bulletSpeed;
        BulletSpeed += _grip.bulletSpeed;
        BulletSpeed += _magazine.bulletSpeed;
        BulletSpeed += _muzzle.bulletSpeed;
        BulletSpeed += _spring.bulletSpeed;

        BulletSpeed = Mathf.Clamp(BulletSpeed, 1f, BulletSpeed);
    }

    private void GetDamage()
    {
        Damage = 0;
        Damage += _barrel.damage;
        Damage += _grip.damage;
        Damage += _magazine.damage;
        Damage += _muzzle.damage;
        Damage += _spring.damage;

        Damage = Mathf.Clamp(Damage, 5, Damage);
    }

    private void GetShootingRate()
    {
        ShootingRate = 0;
        ShootingRate += _barrel.shootingRate;
        ShootingRate += _grip.shootingRate;
        ShootingRate += _magazine.shootingRate;
        ShootingRate += _muzzle.shootingRate;
        ShootingRate += _spring.shootingRate;

        ShootingRate = Mathf.Clamp(ShootingRate, 0.5f, ShootingRate);
    }

    private void GetOverheatRate()
    {
        OverheatRate = 0;
        OverheatRate += _barrel.overheatRate;
        OverheatRate += _grip.overheatRate;
        OverheatRate += _magazine.overheatRate;
        OverheatRate += _muzzle.overheatRate;
        OverheatRate += _spring.overheatRate;
        
        OverheatRate = Mathf.Clamp(OverheatRate, 0.1f, OverheatRate);
    }

    public GameObject GetBullet()
    {
        return _magazine.bullet;
    }
    public GameObject GetMuzzle()
    {
        return _magazine.muzzle;
    }

    public void GetStats()
    {
        GetAccuracy();
        GetBulletLife();
        GetBulletSize();
        GetBulletSpeed();
        GetDamage();
        GetShootingRate();
        GetOverheatRate();
    }
}
