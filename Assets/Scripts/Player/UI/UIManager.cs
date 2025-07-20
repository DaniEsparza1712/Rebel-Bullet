using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField] private PlayerController playerController;
    [SerializeField] private GameObject pauseUI;
    [SerializeField] private GameObject gameUI;

    private void Awake()
    {
        pauseUI.SetActive(false);
        gameUI.SetActive(true);
        
        playerController.OnPause += (sender, args) =>
        {
            pauseUI.SetActive(true);
            gameUI.SetActive(false);
        };
        playerController.OnUnpause += (sender, args) =>
        {
            pauseUI.SetActive(false);
            gameUI.SetActive(true);
        };
    }
}
