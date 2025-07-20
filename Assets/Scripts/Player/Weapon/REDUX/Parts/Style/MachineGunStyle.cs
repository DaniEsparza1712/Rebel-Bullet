using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MachineGunStyle : ChipStyle
{
    [SerializeField] private LineRenderer lineRenderer;
    [SerializeField] private LayerMask shootable;
    
    [Header("Positions")]
    [SerializeField] private Transform bulletSpawn;
    [SerializeField] private Transform aimPoint;
    
    [Header("Shoot Data")]
    [SerializeField] private float shootRate;
    [SerializeField] private float shootDistance;
    [SerializeField] private Attack attack;

    private bool _isShooting;
    private float _lineUpdateRate = 0.01f;
    private float _timer;

    private void Update()
    {
        if(!lineRenderer.enabled)
            return;
        
        _timer += Time.deltaTime;
        if(_timer < _lineUpdateRate)
            return;

        UpdateLineRenderer();
        _timer = 0;
    }

    private void CheckShoot()
    {
        var dir = aimPoint.position - transform.position;
        dir.Normalize();
        if (!Physics.Raycast(bulletSpawn.position, dir, out var hit, shootDistance, shootable))
            return;
        if (hit.transform.gameObject.TryGetComponent<EnemyHittable>(out var hittable))
        {
            var hitPoint = hit.point;
            var startPos = bulletSpawn.position;
            var args = new AttackArgs(attack, hitPoint, dir.normalized, startPos);
            hittable.OnGetHit(args);
        }
    }

    private void UpdateLineRenderer()
    {
        lineRenderer.positionCount = 2;
        lineRenderer.SetPosition(0, bulletSpawn.position);
        lineRenderer.SetPosition(1, aimPoint.position);
    }

    private IEnumerator ShootCoroutine()
    {
        while (_isShooting)
        {
            UpdateLineRenderer();
            CheckShoot();
            yield return new WaitForSeconds(shootRate);
        }
    }
    
    public override void StartShooting()
    {
        _isShooting = true;
        lineRenderer.enabled = true;
        StartCoroutine(ShootCoroutine());
    }

    public override void StopShooting()
    {
        lineRenderer.enabled = false;
        _isShooting = false;
    }
}
