using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class FollowTarget : MonoBehaviour
{
    public GameObject target;
    public NavMeshAgent agent;
    // Start is called before the first frame update
    void Start()
    {
        agent.destination = target.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        agent.destination = target.transform.position;
    }
}
