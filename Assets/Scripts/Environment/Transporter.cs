using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Transporter : MonoBehaviour
{
    [SerializeField] private string sceneName;
    [SerializeField] private string exitName;
    private SceneLoader _sceneLoader;

    private void Start()
    {
        _sceneLoader = GameObject.Find("SceneManager").GetComponent<SceneLoader>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerPrefs.SetString("LastExit", exitName);
            _sceneLoader.LoadNewScene(sceneName);
        }
    }
}
