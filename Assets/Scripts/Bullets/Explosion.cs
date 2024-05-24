using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Explosion : MonoBehaviour
{
    [SerializeField] private int damage;
    [SerializeField] private LayerMask interactionMask;
    [SerializeField] private List<string> targetsTag;

    private void OnTriggerEnter(Collider other)
    {
        if (targetsTag.Contains(other.tag))
        {
            other.GetComponent<LifeSystem>().ApplyDamage(damage);
        }
    }
}
