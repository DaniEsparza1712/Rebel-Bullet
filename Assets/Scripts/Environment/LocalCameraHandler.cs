using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using Unity.VisualScripting;
using UnityEngine;

public class LocalCameraHandler : MonoBehaviour
{
    public float cutsceneTime;
    public CinemachineVirtualCamera vCamera;
    private Manager _manager;

    private void Start()
    {
        _manager = GameObject.Find("Manager").GetComponent<Manager>();
    }

    public void ActivateCamera()
    {
        StartCoroutine(CamCoroutine());
    }

    private IEnumerator CamCoroutine()
    {
        _manager.SetControl(false);
        vCamera.enabled = true;
        yield return new WaitForSeconds((cutsceneTime));
        vCamera.enabled = false;
        _manager.SetControl(true);
    }
}
