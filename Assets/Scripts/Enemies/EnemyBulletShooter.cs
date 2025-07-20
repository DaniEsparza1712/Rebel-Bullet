using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

[Serializable]
public class Shooter
{
    [SerializeField] private Transform aimOrigin;
    public Transform AimOrigin => aimOrigin;
    [SerializeField] private BulletController bulletPrefab;
    public BulletController BulletPrefab { get => bulletPrefab; }
    private List<BulletController> _bullets = new List<BulletController>();
    [SerializeField] private int bulletsAmount;
    public int BulletsAmount { get { return bulletsAmount; } }
    [SerializeField] private LayerMask aimingMask;
    public LayerMask AimingMask { get => aimingMask; }
    [SerializeField] private float aimRange;
    public float AimRange { get => aimRange; }
    [SerializeField] private LineRenderer aimLine;
    private Transform _target;
    private Vector3 _aimDir;

    private void Awake()
    {
        _target = GameObject.FindGameObjectsWithTag("Player")[0].transform;
    }

    public void AddBullet(BulletController bullet)
    {
        _bullets.Add(bullet);
    }
    
    private BulletController GetFromPool()
    {
        foreach (var instBullet in _bullets)
        {
            if(!instBullet.gameObject.activeSelf)
                return instBullet;
        }
        return null;
    }
    
    public void Shoot()
    {
        var instBullet = GetFromPool();
            
        //instBullet.OnShot?.Invoke(this, new BulletArgs(aimOrigin.forward));
        instBullet.transform.parent = null;
        instBullet.transform.position = aimOrigin.position;
        instBullet.gameObject.SetActive(true);
    }

    public void SetAimEnabled(bool enabled)
    {
        Aim();
        aimLine.enabled = enabled;
    }

    public void Aim()
    {
        aimLine.SetPosition(0, aimOrigin.position);
        aimLine.SetPosition(1, GetTargetPosition());
    }
    
    private Vector3 GetTargetPosition()
    {
        _aimDir = aimOrigin.forward;
        if (Physics.Raycast(aimOrigin.position, _aimDir, out var hit, aimRange, aimingMask))
        {
            return hit.point;
        }
        return aimOrigin.position + _aimDir * aimRange;
    }
}

public class EnemyBulletShooter : MonoBehaviour
{
    [Header("Shooters")]
    [SerializeField] List<Shooter> shooters = new List<Shooter>();
    
    [Header("Targets")]
    private bool _isAiming;
    [SerializeField] private LayerMask playerMask;
    
    private EnemyStatesMachine _stateMachine;

    private void Awake()
    {
        _stateMachine = GetComponent<EnemyStatesMachine>();
        
        _stateMachine.OnStartAim += (sender, args) =>
        {
            _isAiming = true;
            ChangeAim(true);
        };
        _stateMachine.OnEndAim += (sender, args) =>
        {
            _isAiming = false;
            ChangeAim(false);
        };
        _stateMachine.OnDeath += (sender, args) =>
        {
            _isAiming = false;
            ChangeAim(false);
        };
    }

    public void ShootWithIndex(int index)
    {
        shooters[index].Shoot();
    }
    
    private void FillShooters()
    {
        foreach (var shooter in shooters)
        {
            for (int i = 0; i < shooter.BulletsAmount; i++)
            {
                var instBullet = Instantiate(shooter.BulletPrefab);
                instBullet.transform.SetParent(shooter.AimOrigin);
                instBullet.gameObject.SetActive(false);
                shooter.AddBullet(instBullet);
            }
        }
    }

    private void ChangeAim(bool enable)
    {
        foreach (var shooter in shooters)
            shooter.SetAimEnabled(enable);
    }

    // Start is called before the first frame update
    void Start()
    {
        FillShooters();
    }

    // Update is called once per frame
    void Update()
    {
        if (!_isAiming)
            return;
        foreach (var shooter in shooters)
            shooter.Aim();
    }
    
    public bool HasClearShot()
    {
        foreach (var shooter in shooters)
        {
            var pos = shooter.AimOrigin.position;
            var dir = shooter.AimOrigin.forward.normalized;

            if (Physics.Raycast(pos, dir, shooter.AimRange, playerMask))
                return true;
        }

        return false;
    }
    
}
