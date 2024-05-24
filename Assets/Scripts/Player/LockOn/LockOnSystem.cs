using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.Timeline.AnimationPlayableAsset;

public class LockOnSystem : MonoBehaviour
{
    public enum LockMode
    {
        Free,
        Target
    }
    public LockMode lockMode;
    public float maxXRotation = 180;
    public float minXRotation = -180;
    public float currentXRotation;
    public float rotationMod = 25;
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
