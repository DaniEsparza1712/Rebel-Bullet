using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "WeaponParts/Scope")]
public class GunScope : ScriptableObject
{
    public string scopeName;
    [TextArea]
    public string scopeDescription;
    public GameObject scopePrefab;
}
