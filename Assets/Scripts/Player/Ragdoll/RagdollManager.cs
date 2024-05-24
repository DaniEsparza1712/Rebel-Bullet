using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class RagdollManager : MonoBehaviour
{
    public Rigidbody mainRb;
    public Collider mainCollider;
    private List<Rigidbody> _rbs = new();
    private List<Collider> _colliders = new();
    private List<CharacterJoint> _joints;
    public CharacterController CharacterController;
    public Animator Animator;

    private void Start()
    {
        foreach (var rb in GetComponentsInChildren<Rigidbody>())
        {
            if (rb != mainRb)
                _rbs.Add(rb);
        }

        foreach (var col in GetComponentsInChildren<Collider>())
        {
            if(col.GetType() != typeof(CharacterController) && col != mainCollider)
                _colliders.Add(col);
        }

        ChangeRbs(false);
        ChangeColliders(false);
    }
    
    private void ChangeRbs(bool change)
    {
        mainRb.isKinematic = change;
        foreach (var rb in _rbs)
        {
            rb.isKinematic = !change;
        }
    }

    private void ChangeColliders(bool change)
    {
        mainCollider.enabled = !change;
        foreach (var col in _colliders)
        {
            col.enabled = change;
        }
    }

    public void DeathEvent()
    {
        StartCoroutine(WaitForDeath());
    }

    public IEnumerator WaitForDeath()
    {
        yield return new WaitForEndOfFrame();
        Animator.enabled = false;
        CharacterController.enabled = false;
            
        ChangeRbs(true);
        ChangeColliders(true);
    }
}
