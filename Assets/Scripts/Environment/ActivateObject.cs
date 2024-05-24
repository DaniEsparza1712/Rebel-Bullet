using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivateObject : MonoBehaviour
{
    [SerializeField] private GameObject objectToActivate;

    public void ObjectSetActive(bool active)
    {
        objectToActivate.SetActive(active);
    }
}
