using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ComponentPickUp : MonoBehaviour
{
    [SerializeField] private WeaponComponent component;
    public UnityEvent onPickUpEvent;
    private ComponentInitializer _componentInitializer;
    // Start is called before the first frame update
    void Start()
    {
        _componentInitializer = GameObject.Find("Sam").GetComponentInChildren<ComponentInitializer>();
        onPickUpEvent.AddListener(AddToInventory);
    }

    private void OnTriggerEnter(Collider other)
    {
        onPickUpEvent.Invoke();
    }

    private void AddToInventory()
    {
        _componentInitializer.AddComponent(component);
    }
}
