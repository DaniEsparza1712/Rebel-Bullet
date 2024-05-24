using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EventArea : MonoBehaviour
{
    public UnityEvent onEnterAreaEvent;
    public UnityEvent onExitAreaEvent;
    [SerializeField] private string targetTag;
    [SerializeField] private AudioClip music;
    private MusicManager _musicManager;
    [SerializeField] private bool changeMusic;
    [SerializeField] private bool loopMusic;

    private void Start()
    {
        _musicManager = Camera.main.GetComponent<MusicManager>();
        if (changeMusic)
        {
            onEnterAreaEvent.AddListener(delegate { _musicManager.ChangeMusic(music);});
            _musicManager.ChangeLoop(loopMusic);
        }
}

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag(targetTag))
            onEnterAreaEvent.Invoke();
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.CompareTag(targetTag))
            onExitAreaEvent.Invoke();
    }
}
