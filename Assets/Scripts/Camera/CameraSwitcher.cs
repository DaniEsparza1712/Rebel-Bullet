using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraSwitcher : MonoBehaviour
{
    public enum Cameras{
        main,
        lockOn,
        death
    }
    public CinemachineFreeLook mainCamera;
    public CinemachineVirtualCamera lockOnCamera;
    public CinemachineVirtualCamera deathCamera;
    public CinemachineBrain brain;
    // Start is called before the first frame update
    public void SwitchCamera(Cameras cameras){
        switch(cameras){
            case Cameras.main:
                mainCamera.enabled = true;
                lockOnCamera.enabled = false;
                deathCamera.enabled = false;
                break;
            case Cameras.lockOn:
                mainCamera.enabled = false;
                lockOnCamera.enabled = true;
                deathCamera.enabled = false;
                break;
            case Cameras.death:
                mainCamera.enabled = false;
                lockOnCamera.enabled = false;
                deathCamera.enabled = true;
                break;
        }
    }
}
