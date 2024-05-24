using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Electrocute : MonoBehaviour
{
    [SerializeField] private int damage;
    [SerializeField] private float lifeTime;
    private float _timer;
    private LifeSystem _targetLife;
    private Vector3 _pos;

    private void Update()
    {
        _timer += Time.deltaTime;
        if (_timer >= lifeTime)
        {
            Destroy(gameObject);
        }
    }

    private void FixedUpdate()
    {
        _targetLife.ApplyDamage(damage);
    }

    public void SetTarget(LifeSystem lifeSystem)
    {
        _targetLife = lifeSystem;
    }
}
