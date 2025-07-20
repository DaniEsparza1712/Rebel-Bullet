using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum AttackElement
{
    None,
    Fire,
    Lightning,
    Concussive
}

[CreateAssetMenu(menuName = "ScriptableObjects/Attacks/Attack")]
public class Attack : ScriptableObject
{
    public AttackElement element;
    public float damage;
    public int elementPoints;

}
