using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBulletManager : MonoBehaviour
{
    public enum ShootingStage
    {
        idle,
        shooting,
        waiting
    }
    ShootingStage shootingStage;

    public BulletPooler bulletPooler;
    public BulletPooler invincibleBulletPooler;
    public float shootingRate;
    public float shootingTime;
    public float shootingWaitTime;
    public int invincibleBulletNum;
    private int bulletNum;

    private Transform _target;
    CharacterController playerController;
    private float timer;
    private float shootTimer;
    public bool isShooting;

    private Manager _manager;
    // Start is called before the first frame update
    void Start()
    {
        timer = 0;
        shootTimer = 0;
        bulletNum = 1;
        ChangeStage(ShootingStage.shooting);
        _target = GameObject.Find("Sam").GetComponent<Transform>();

        _manager = GameObject.Find("Manager").GetComponent<Manager>();
    }

    // Update is called once per frame
    void Update()
    {
        if(shootingStage == ShootingStage.shooting && isShooting && _manager.hasControl)
        {
            timer += Time.deltaTime;
            shootTimer += Time.deltaTime;
            if(shootTimer > shootingRate)
            {
                if(bulletNum >= invincibleBulletNum)
                {
                    bulletNum = 1;
                    invincibleBulletPooler.PullBullet(transform.position, transform.forward);
                }
                else
                {
                    bulletPooler.PullBullet(transform.position, transform.forward);
                }

                bulletNum++;
                shootTimer = 0;
            }
            if(timer > shootingTime)
            {
                bulletNum = 1;
                ChangeStage(ShootingStage.waiting);
                shootTimer = 0;
                timer = 0;
            }
        }
        else if(shootingStage == ShootingStage.waiting && _manager.hasControl)
        {
            timer += Time.deltaTime;
            if(timer > shootingWaitTime)
            {
                ChangeStage(ShootingStage.shooting);
                timer = 0;
            }
        }
    }

    private void FixedUpdate()
    {
        transform.forward = (_target.position + Vector3.up * 0.5f - transform.position).normalized;
    }

    public void ChangeStage(ShootingStage newState)
    {
        shootingStage = newState;
    }

}
