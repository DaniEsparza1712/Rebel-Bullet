using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloorVoid : MonoBehaviour
{
    [SerializeField] private Transform spawnPos;
    [SerializeField] private int damage;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            other.transform.position = spawnPos.position;
            other.GetComponent<LifeSystem>().ApplyDamage(damage);
        }
    }
}
