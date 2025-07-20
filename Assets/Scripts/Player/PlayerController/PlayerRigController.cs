using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class PlayerRigController : MonoBehaviour
{
    private PlayerController _playerController;
    [SerializeField]
    private Rig aimRig;

    private void Awake()
    {
        _playerController = GetComponent<PlayerController>();
        _playerController.OnGunOut += (sender, args) =>
        {
            aimRig.weight = 1;
        };
        _playerController.OnGunIn += (sender, args) =>
        {
            aimRig.weight = 0;
        };
    }
}
