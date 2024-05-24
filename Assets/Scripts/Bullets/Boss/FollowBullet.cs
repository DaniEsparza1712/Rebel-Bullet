using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowBullet : MonoBehaviour
{
    private Transform _playerTransform;
    private Vector3 _targetPos;
    private void Start()
    {
        _playerTransform = GameObject.Find("Sam").GetComponent<Transform>();
    }

    private void FixedUpdate()
    {
        _targetPos = _playerTransform.position + Vector3.up * 0.5f;
        transform.forward = (_targetPos - transform.position).normalized;
    }
}
