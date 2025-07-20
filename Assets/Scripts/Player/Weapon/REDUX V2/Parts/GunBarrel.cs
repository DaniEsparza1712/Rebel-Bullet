using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Stats:
 * 
 * Shooting:
 * Fire rate
 * Projectile speed
 * Projectile type
 * Recoil
 * Damage
 *
 * Heating:
 * Heat Rate
 * Heat Dissipation
 * Max Heat
 * Overheat Penalty
 *
 * Control:
 * Movement Speed while Aiming
 *
 * Special Stats:
 * Element type
 * Status Chance
 * Pierce count
 */

[CreateAssetMenu(menuName = "WeaponParts/Barrel")]
public class GunBarrel : ScriptableObject
{
    public string barrelName;
    [TextArea]
    public string barrelDescription;
    public GameObject barrelPrefab;
    public float fireRate = 0.5f;
    public float projectileSpeed = 10f;
    public float heatRate  = 0.5f;
    public float statusChance = 0.5f;
    public int pierceCount = 1;
}
