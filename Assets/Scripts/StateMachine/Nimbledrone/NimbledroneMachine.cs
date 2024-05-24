using UnityEngine;
using UnityEngine.AI;

public class NimbledroneMachine : StateMachine
{
    [HideInInspector]
    public NimbledroneFollow Follow;
    [HideInInspector] public NimbledroneShoot Shoot;
    [HideInInspector] public NimbledroneIdle Idle;
    
    [Header("Movement")]
    public NavMeshAgent Agent;
    public float speed;
    
    [Header("Distances")]
    public float Distance;
    public float sightDistance;
    public float shootingDistance;

    public EnemyBulletManager bulletManager;
    [HideInInspector]
    public Transform target;
    private void Awake()
    {
        Idle = new NimbledroneIdle(this);
        Follow = new NimbledroneFollow(this);
        Shoot = new NimbledroneShoot(this);
        
        currentState = Idle;
        currentState.Enter();
        target = GameObject.Find("Sam").GetComponent<Transform>();
    }

    public float DistanceToTarget()
    {
        return Vector3.Distance(target.position, transform.position);
    }
}
