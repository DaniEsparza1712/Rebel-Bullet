using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathSceneManager : MonoBehaviour
{
    [SerializeField] private float waitTime;
    [SerializeField] private LifeSystem lifeSystem;
    [SerializeField] private string scene;
    private SceneLoader _sceneLoader;
    private List<GameObject> _keepOnLoad = new();
    // Start is called before the first frame update
    void Start()
    {
        _keepOnLoad.Clear();
        var keep = GameObject.FindObjectsOfType<KeepOnLoad>();
        foreach (var k in keep)
        {
            if(k.gameObject.name != "SceneManager")
                _keepOnLoad.Add(k.gameObject);
        }
        _sceneLoader = GameObject.Find("SceneManager").GetComponent<SceneLoader>();
        lifeSystem.OnDeath.AddListener(LoadDeathScene);
    }

    public void LoadDeathScene()
    {
        _sceneLoader.LoadDestroy(scene, waitTime, _keepOnLoad);
    }
}
