using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = "ScriptableObjects/Component/Magazine")]
public class MagazineComponent : WeaponComponent
{
    public GameObject bullet;
    public GameObject muzzle;
}
