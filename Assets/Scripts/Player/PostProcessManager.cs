using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class PostProcessManager : MonoBehaviour
{
    [SerializeField] private Volume defaultVolume;
    [SerializeField] private Volume detectiveVolume;
    private UniversalAdditionalCameraData _cameraData;
    
    
    void Awake()
    {
        _cameraData = Camera.main.GetComponent<UniversalAdditionalCameraData>();
        
        defaultVolume.gameObject.SetActive(true);
        detectiveVolume.gameObject.SetActive(true);
        
        defaultVolume.enabled = true;
        detectiveVolume.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            var active = !detectiveVolume.enabled;
            detectiveVolume.enabled = active;
            
            var rendererIndex = active ? 1 : 0; 
            _cameraData.SetRenderer(rendererIndex);
        }
    }
}
