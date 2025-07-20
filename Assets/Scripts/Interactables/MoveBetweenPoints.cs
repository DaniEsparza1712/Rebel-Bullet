using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Rendering.PostProcessing;

[RequireComponent(typeof(Rigidbody))]
public class MoveBetweenPoints : MonoBehaviour
{
    public UnityEvent onHasMoved;
    [SerializeField] private List<Transform> points = new List<Transform>();
    [SerializeField] [MinMax(0.0f, 1.0f)] private float moveSpeed = 0.5f;
    [SerializeField] private float moveRate = 0.5f;
    private Rigidbody _rb;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
    }

    public void MoveToPoint(int index)
    {
        StartCoroutine(MoveToPointCoroutine(index));
    }

    private IEnumerator MoveToPointCoroutine(int index)
    {
        var lerpIndex = 0.0f;
        var originalPos = _rb.position;
        var targetPos = points[index].position;
        while (lerpIndex < 1.0f)
        {
            lerpIndex += moveSpeed;
            var currentTargetPos = Vector3.Lerp(originalPos, targetPos, lerpIndex);
            _rb.MovePosition(currentTargetPos);
            yield return new WaitForSeconds(moveRate);
        }
        onHasMoved.Invoke();
    }
}
