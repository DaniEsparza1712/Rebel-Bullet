using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class WeaponComponent : ScriptableObject
{
    public string componentName;
    public float shootingRate;

    public int damage;

    public float bulletSpeed;

    public float bulletSize;

    public float accuracy;

    public float overheatRate;

    public float bulletLife;
}
