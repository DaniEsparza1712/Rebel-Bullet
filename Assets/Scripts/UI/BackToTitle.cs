using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackToTitle : MonoBehaviour
{
    [SerializeField] private SceneLoader sceneLoader; 
    // Start is called before the first frame update
    void Start()
    {
        sceneLoader = GameObject.Find("SceneManager").GetComponent<SceneLoader>();
    }

    public void LoadTitle()
    {
        sceneLoader.LoadNewScene("TitleScreen");
    }
}
