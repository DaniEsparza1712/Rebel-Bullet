using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class MusicManager : MonoBehaviour
{
    private AudioSource _source;

    private void Start()
    {
        _source = GetComponent<AudioSource>();
    }

    public void ChangeMusic(AudioClip clip)
    {
        _source.clip = clip;
        _source.Play();
    }

    public void ChangeLoop(bool looping)
    {
        _source.loop = looping;
    }
}
