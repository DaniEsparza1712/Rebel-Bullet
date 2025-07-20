using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponStats
{
    //Shooting stats
    private float _fireRate;
    public float FireRate => _fireRate;
    private float _projectileSpeed;
    public float ProjectileSpeed => _projectileSpeed;
    private GunBody.ProjectileType _projectileType;
    public GunBody.ProjectileType ProjectileType => _projectileType;
    private float _recoil;
    public float Recoil => _recoil;
    private float _damage;
    public float Damage => _damage;
    
    //Heating stats
    private float _heatRate;
    public float HeatRate => _heatRate;
    private float _maxHeat;
    public float MaxHeat => _maxHeat;
    private float _heatDissipation;
    public float HeatDissipation => _heatDissipation;
    private float _overheatTime;
    public float OverheatTime => _overheatTime;
    
    //Movement stats
    private float _weaponWeight;
    public float WeaponWeight => _weaponWeight;
    
    //Special Stats
    private float _statusChance;
    public float StatusChance => _statusChance;
    private int _pierceCount;
    public int PierceCount => _pierceCount;
    private GunCore.ElementType _elementType;
    public GunCore.ElementType ElementType => _elementType;
    
    //Attack
    private Attack _currentAttack;
    public Attack CurrentAttack => _currentAttack;

    public WeaponStats(GunBody body, GunBarrel barrel, GunGrip grip, GunStock stock, GunCore core)
    {
        _currentAttack = ScriptableObject.CreateInstance<Attack>();
        UpdateStats(body, barrel, grip, stock, core);
    }

    public void UpdateStats(GunBody body, GunBarrel barrel, GunGrip grip, GunStock stock, GunCore core)
    {
        _fireRate = Mathf.Clamp(1 / (barrel.fireRate + body.fireRate), 0.05f, 1f);
        _projectileSpeed = barrel.projectileSpeed + body.projectileSpeed;
        _projectileType = body.projectileType;
        _recoil = grip.recoil + stock.recoil;
        _damage = body.damage + core.damage;
        
        _heatRate = barrel.heatRate;
        _maxHeat = stock.maxHeat + body.maxHeat;
        _heatDissipation = grip.heatDissipation + body.heatDissipation;
        _overheatTime = 1.5f;

        _weaponWeight = body.movementSpeed + grip.movementSpeed;
        
        _statusChance = barrel.statusChance + stock.statusChance;
        _pierceCount = barrel.pierceCount;
        _elementType = core.elementType;

        _currentAttack.damage = _damage;
        _currentAttack.element = AttackElement.None;
    }
    
}
