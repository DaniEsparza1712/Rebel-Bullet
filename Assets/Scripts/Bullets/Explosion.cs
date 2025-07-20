using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Explosion : MonoBehaviour
{
    [SerializeField] private Attack attack;
    [SerializeField] private LayerMask interactionMask;
    [SerializeField] private List<string> targetsTag;

    private void OnTriggerEnter(Collider other)
    {
        if (targetsTag.Contains(other.tag))
        {
            var dir = (other.transform.position - transform.position) + Vector3.up * 1.5f;
            var args = new AttackArgs(attack, other.ClosestPoint(transform.position), dir.normalized, transform.position);
            other.GetComponent<EnemyHittable>().OnGetHit(args);
        }
    }
}
