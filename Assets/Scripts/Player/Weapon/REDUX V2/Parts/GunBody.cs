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

[CreateAssetMenu(menuName = "WeaponParts/Body")]
public class GunBody : ScriptableObject
{
    public enum ProjectileType
    {
        Default,
        Laser,
        Grenade
    }
    
    [Header("Info")]
    public string bodyName;
    [TextArea]
    public string bodyDescription;
    
    [Header("Stats")]
    public GameObject bodyPrefab;
    public float damage = 10f;
    public ProjectileType projectileType = ProjectileType.Default;
    public float projectileSpeed = 10f;
    public float maxHeat = 100f;
    public float movementSpeed = 7f;
    public float heatDissipation = 5f;
    public float fireRate = 0.25f;
    
    [Header("Positions")]
    public Vector3 barrelPosition;
    public Vector3 stockPosition;
    public Vector3 scopePosition;
    public Vector3 gripPosition;
}
