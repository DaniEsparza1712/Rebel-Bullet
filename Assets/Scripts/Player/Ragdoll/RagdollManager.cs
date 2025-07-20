using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class RagdollManager : MonoBehaviour
{
    public Rigidbody mainRb;
    public Collider mainCollider;
    private List<Rigidbody> _rbs = new();
    public Rigidbody[] Rigidbodies => _rbs.ToArray();
    private List<Collider> _colliders = new();
    public Collider[] Colliders => _colliders.ToArray();
    private List<CharacterJoint> _joints;
    [SerializeField] private Rigidbody hips;
    public CharacterController CharacterController;
    public Animator Animator;
    [SerializeField] private LayerMask floorMask;

    public EventHandler OnRagdollEnabled;
    public EventHandler OnRagdollDisabled;

    private bool _canGetUp = false;

    private void Awake()
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

    public void EnableRagdoll()
    {
        Animator.enabled = false;
        if(CharacterController)
            CharacterController.enabled = false;
        ChangeRbs(true);
        ChangeColliders(true);
        _canGetUp = false;
        StartCoroutine("SetGetUp");
        OnRagdollEnabled?.Invoke(this, EventArgs.Empty);
    }

    private IEnumerator SetGetUp()
    {
        yield return new WaitForSeconds(0.2f);
        _canGetUp = true;
    }

    private bool HipsStatic()
    {
        return hips.velocity.magnitude <= 0.1f;
    }
    
    private bool OnGround()
    {
        var distance = hips.gameObject.GetComponent<Collider>().bounds.extents.magnitude;
        if (Physics.Raycast(hips.transform.position, -Vector3.up, out RaycastHit hit, distance, floorMask))
        {
            Debug.Log(hit.collider.gameObject.name);
            return true;
        }
        return false;
    }

    public bool CanGetUp()
    {
        return OnGround() && HipsStatic() && _canGetUp;
    }

    public void UpdateTransform()
    {
        var ogHipsPos = hips.transform.position;
        var ogHipsRot = hips.transform.rotation;

        var fwd = Vector3.ProjectOnPlane(-hips.transform.up, Vector3.up).normalized;
        transform.forward = fwd;
        
        if(Physics.Raycast(ogHipsPos, Vector3.down, out RaycastHit hit, floorMask))
            ogHipsPos = hit.point;

        transform.position = ogHipsPos;
        hips.transform.position = ogHipsPos;
        //hips.transform.rotation = ogHipsRot;
    }

    public void ApplyForceAtPoint(float force, Vector3 forceDirection, Vector3 point)
    {
        var hitRB = Rigidbodies.OrderBy(rb => Vector3.Distance(rb.position, point)).First();
        hitRB.AddForceAtPosition(forceDirection.normalized * force, point, ForceMode.Impulse);
    }
    
    public void DisableRagdoll()
    {
        Animator.enabled = true;
        if(CharacterController)
            CharacterController.enabled = true;
        ChangeRbs(false);
        ChangeColliders(false);
        OnRagdollDisabled?.Invoke(this, EventArgs.Empty);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawRay(new Ray(hips.transform.position, -Vector3.up * 0.1f));
    }
}
