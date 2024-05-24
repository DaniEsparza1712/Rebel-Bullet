using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hit : MonoBehaviour
{
    [SerializeField] private List<string> tagList = new();
    [SerializeField] private int damage;
    [SerializeField] private GameObject hitParticles;
    [SerializeField] private bool knockOut;
    [SerializeField] private AudioSource source;
    [SerializeField] private AudioClip clip;
    private void OnTriggerEnter(Collider other)
    {
        if (tagList.Contains(other.tag))
        {
            other.GetComponent<LifeSystem>().ApplyDamage(-Mathf.Abs(damage));
            Instantiate(hitParticles, other.ClosestPoint(transform.position), hitParticles.transform.rotation);
            source.PlayOneShot(clip);
            if (knockOut)
            {
                other.gameObject.SendMessage("KO", null, SendMessageOptions.DontRequireReceiver);
            }
        }
    }
}
