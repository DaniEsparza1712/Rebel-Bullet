using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Aim : MonoBehaviour
{
    public float castDist;
    public float speed;
    public LayerMask castsWith;

    private Vector3 _targetPos;
    private Vector3 _originPos;
    private Transform _camTransform;
    // Start is called before the first frame update
    void Start()
    {
        _camTransform = Camera.main.transform;
    }

    // Update is called once per frame
    void Update()
    {
        _originPos = _camTransform.position + _camTransform.forward * 2.5f;
        if(Physics.Raycast(_originPos, _camTransform.forward, out RaycastHit hit, castDist, castsWith)){
            _targetPos = hit.point;
        }
        else
            _targetPos = _camTransform.position + _camTransform.forward * castDist;
        transform.position = _targetPos;
    }
}
