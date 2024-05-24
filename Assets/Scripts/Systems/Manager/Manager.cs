using UnityEditor;
using UnityEngine;

public class Manager : MonoBehaviour
{
    public bool hasControl;

    public void SetControl(bool control)
    {
        hasControl = control;
    }
}
