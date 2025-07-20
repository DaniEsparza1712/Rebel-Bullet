using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpringLineRenderer : MonoBehaviour
{
    private LineRenderer _lineRenderer;
    private Transform _obj1Transform;
    private Transform _obj2Transform;
    private Vector3 _obj1LocalPosition;
    private Vector3 _obj2LocalPosition;

    private void Awake()
    {
        var lineContainer = new GameObject("LineContainer");
        lineContainer.transform.SetParent(transform);
        _lineRenderer = lineContainer.AddComponent<LineRenderer>();
        _lineRenderer.useWorldSpace = true;
    }

    public void SetVisuals(Material material, float width)
    {
        _lineRenderer.material = material;
        _lineRenderer.startWidth = width;
        _lineRenderer.endWidth = width;
    }

    public void SetObjects(Transform obj1, Vector3 localPos1, Transform obj2, Vector3 localPos2)
    {
        _obj1Transform = obj1;
        _obj1LocalPosition = localPos1;
        
        _obj2Transform = obj2;
        _obj2LocalPosition = localPos2;
    }

    private void OnDestroy()
    {
        Destroy(_lineRenderer);
    }

    // Update is called once per frame
    void Update()
    {
        _lineRenderer.SetPosition(0, _obj1Transform.TransformPoint(_obj1LocalPosition));
        _lineRenderer.SetPosition(1, _obj2Transform.TransformPoint(_obj2LocalPosition));
    }
}
