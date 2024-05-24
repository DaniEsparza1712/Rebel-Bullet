using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class PlaySound : MonoBehaviour
{
    [SerializeField] AudioSource audioSource;
   

    public void playSound()
    {
        audioSource.Play();
    }
}
