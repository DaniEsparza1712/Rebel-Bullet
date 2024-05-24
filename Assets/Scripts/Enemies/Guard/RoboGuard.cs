using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoboGuard : MonoBehaviour
{
    private Transform _playerTransform;
    private Vector3 _facePos;
    // Start is called before the first frame update
    void Start()
    {
        _playerTransform = GameObject.Find("Sam").GetComponent<Transform>();
        _facePos = _playerTransform.position - transform.position;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        _facePos = _playerTransform.position - transform.position;
        _facePos.y = transform.position.y;
        transform.forward = _facePos;
    }
}
