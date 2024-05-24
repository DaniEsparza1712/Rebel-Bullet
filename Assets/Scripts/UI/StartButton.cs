using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StartButton : MonoBehaviour
{
    [SerializeField] private SceneLoader sceneManager;
    public string scene;
    // Start is called before the first frame update
    void Start()
    {
        sceneManager = GameObject.Find("SceneManager").GetComponent<SceneLoader>();
        GetComponent<Button>().onClick.AddListener(delegate { sceneManager.LoadNewScene(scene);});
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
