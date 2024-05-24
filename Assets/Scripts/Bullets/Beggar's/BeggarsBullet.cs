using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeggarsBullet : MonoBehaviour
{
    public List<string> targetTags = new();

    private void OnTriggerEnter(Collider other)
    {
        if (targetTags.Contains(other.tag))
        {
            other.gameObject.SetActive(false);
        }
    }
}
