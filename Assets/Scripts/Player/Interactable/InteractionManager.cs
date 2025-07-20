using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionManager : MonoBehaviour
{
    [SerializeField] private float interactionRadius;
    [SerializeField] private Transform castPos;
    [SerializeField] private LayerMask interactionLayer;
    
    private PlayerController _playerController;
    private Interactable _currentInteractable;

    private void Awake()
    {
        _playerController = GetComponent<PlayerController>();
        _playerController.OnInteract += (sender, args) =>
        {
            if(_currentInteractable != null)
                _currentInteractable.Interact();
        };
    }

    // Start is called before the first frame update
    void Start()
    {
        _currentInteractable = null;
    }
    
    void FixedUpdate()
    {
        var interactableObjects = Physics.OverlapSphere(castPos.position, interactionRadius, interactionLayer);
        if (interactableObjects.Length > 0)
        {
            for (int i = 0; i < interactableObjects.Length; i++)
            {
                if (interactableObjects[i].TryGetComponent<Interactable>(out var interactable))
                {
                    ChangeInteractable(interactable);
                }
            }
        }
        else
        {
            SetNullInteractable();  
        }
            
    }

    private void ChangeInteractable(Interactable interactable)
    {
        if (_currentInteractable == interactable)
            return;
        if(_currentInteractable != null)
            _currentInteractable.OnDetectRelease();
        _currentInteractable = interactable;
        _currentInteractable.OnDetect();
        Debug.Log(_currentInteractable);
    }

    private void SetNullInteractable()
    {
        if(_currentInteractable != null)
            _currentInteractable.OnDetectRelease();
        _currentInteractable = null;
    }
}
