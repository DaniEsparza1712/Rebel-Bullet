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

[CreateAssetMenu(menuName = "WeaponParts/Grip")]

public class GunGrip : ScriptableObject
{
    public string gripName;
    [TextArea]
    public string gripDescription;
    public GameObject grip;
    public float recoil = 0.5f;
    public float heatDissipation  = 0.5f;
    public float movementSpeed = 3.5f;
}
