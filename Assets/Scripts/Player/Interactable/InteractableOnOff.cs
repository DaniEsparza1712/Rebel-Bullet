using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class InteractableOnOff : MonoBehaviour, Interactable
{
    public UnityEvent onEvent;
    public UnityEvent offEvent;
    public UnityEvent onDetectEvent;
    public UnityEvent onReleaseDetectEvent;
    [SerializeField] private bool interactableOn;
    [SerializeField] private bool canInteract;

    private void Awake()
    {
        
    }

    public void Interact()
    {
        if (!canInteract)
            return;
        Debug.Log("Interacted");
        interactableOn = !interactableOn;
        if(interactableOn)
            onEvent.Invoke();
        else
            offEvent.Invoke();
    }

    public void OnDetect()
    {
        onDetectEvent.Invoke();
    }

    public void OnDetectRelease()
    {
        onReleaseDetectEvent.Invoke();
    }

    public void SetCanInteract(bool can)
    {
        canInteract = can;
    }

    public void SetInteractableOn(bool on)
    {
        interactableOn = on;
    }
}
