using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//In charge of defining shooting method.
[System.Serializable]
public abstract class ChipStyle : MonoBehaviour
{
    public abstract void StartShooting();
    public abstract void StopShooting();
}
