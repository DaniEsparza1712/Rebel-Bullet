using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponStatsUI : MonoBehaviour
{
    [SerializeField] private WeaponManager weaponManager;
    [SerializeField] private StatUIDataContainer statUIDataContainerPrefab;
    [SerializeField] private RectTransform statsContainer;
    
    private readonly Dictionary<string, StatUIDataContainer> _statContainers = new();
    private readonly Dictionary<string, System.Func<object>> _statValueGetters = new();

    private void Start()
    {
        CreateStatsContainer();
        SetStatValues();
        weaponManager.OnChangeComponent += (sender, args) =>
        {
            Debug.Log("ChangedComponent");
            SetStatValues();
        };  
    }

    private void CreateStatsContainer()
    {
        var weaponStats = weaponManager.CurrentStats;
        AddStat("Fire Rate", () => weaponStats.FireRate);
        AddStat("Projectile Speed", () => weaponStats.ProjectileSpeed);
        AddStat("Projectile Type", () => weaponStats.ProjectileType);
        AddStat("Recoil", () => weaponStats.Recoil);
        AddStat("Damage", () => weaponStats.Damage);
        
        AddStat("Heat Rate", () => weaponStats.HeatRate);
        AddStat("Max Heat", () => weaponStats.MaxHeat);
        AddStat("Heat Dissipation", () => weaponStats.HeatDissipation);
        AddStat("Overheat Time", () => weaponStats.OverheatTime);
        
        AddStat("Status Chance", () => weaponStats.StatusChance);
        AddStat("Pierce Count", () => weaponStats.PierceCount);
        AddStat("Element Type", () => weaponStats.ElementType);
    }

    private void AddStat(string statName, System.Func<object> valueGetter)
    {
        var container = Instantiate(statUIDataContainerPrefab, statsContainer);
        container.SetStatName(statName);
        _statContainers[statName] = container;
        
        _statValueGetters[statName] = valueGetter;
    }

    private void SetStatValues()
    {
        foreach (var statValue in _statValueGetters)
        {
            _statContainers[statValue.Key].SetStatValue(statValue.Value.Invoke().ToString());
        }
    }
}
