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

[CreateAssetMenu(menuName = "WeaponParts/Stock")]

public class GunStock : ScriptableObject
{
    public string stockName;
    [TextArea]
    public string stockDescription;
    public GameObject stock;
    public float recoil = 0.5f;
    public float maxHeat = 50f;
    public float statusChance = 0.5f;
}
