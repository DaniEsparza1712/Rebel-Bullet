using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "WeaponParts/Core")]
public class GunCore : ScriptableObject
{
    public enum ElementType
    {
        Energy,
        Fire,
        Critical,
        Poison,
        Lightning
    }

    public string coreName;
    [TextArea]
    public string coreDescription;
    public BulletController bulletPrefab;
    public ElementType elementType;
    public float damage;
    [ColorUsage(false, true)]
    public Color elementColor;
}
