using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cannon : MonoBehaviour
{
    Transform playerTransform;
    CharacterController playerController;
    Vector3 shootPos;

    public bool isShooting;
    // Start is called before the first frame update
    void Start()
    {
        isShooting = false;
        playerTransform = GameObject.Find("Sam").GetComponent<Transform>();
        playerController = GameObject.Find("Sam").GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        if (isShooting)
        {
            shootPos = playerTransform.position + (playerController.height/2 * Vector3.up);
            transform.rotation = Quaternion.LookRotation(shootPos - transform.position);
        }
    }
}
