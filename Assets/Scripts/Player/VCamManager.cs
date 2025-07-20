using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class VCamManager : MonoBehaviour
{
    [SerializeField]
    private CinemachineFreeLook freeLook;
    [SerializeField]
    private CinemachineVirtualCamera aimCamera;
    [SerializeField]
    private PlayerController playerController;
    [SerializeField]
    private CinemachineVirtualCamera ragdollCamera;
    
    private RagdollManager _ragdollManager;

    private void Awake()
    {
        _ragdollManager = playerController.gameObject.GetComponent<RagdollManager>();
        
        playerController.OnAimStarted += (sender, args) =>
        {
            freeLook.gameObject.SetActive(false);
            aimCamera.gameObject.SetActive(true);
        };
        playerController.OnAimEnded += (sender, args) =>
        {
            freeLook.gameObject.SetActive(true);
            aimCamera.gameObject.SetActive(false);
        };
        _ragdollManager.OnRagdollEnabled += (sender, args) =>
        {
            freeLook.gameObject.SetActive(false);
            aimCamera.gameObject.SetActive(false);
            ragdollCamera.gameObject.SetActive(true);
        };
        _ragdollManager.OnRagdollDisabled += (sender, args) =>
        {
            freeLook.gameObject.SetActive(true);
            aimCamera.gameObject.SetActive(false);
            ragdollCamera.gameObject.SetActive(false);
        };
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
