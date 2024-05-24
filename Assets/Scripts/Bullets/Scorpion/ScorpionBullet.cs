using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScorpionBullet : MonoBehaviour
{
    [SerializeField] private GameObject electrocutedObject;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            GameObject electro = Instantiate(electrocutedObject, other.ClosestPoint(transform.position),
                electrocutedObject.transform.rotation);
            electro.transform.SetParent(other.transform);
            electro.GetComponent<Electrocute>().SetTarget(other.GetComponent<LifeSystem>());
        }
    }
}
