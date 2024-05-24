using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LifeBar : MonoBehaviour
{
    public Image lifeImage;
    public LifeSystem lifeSystem;
    public bool faceCamera;

    // Update is called once per frame
    void Update()
    {
        if(faceCamera)
        {
            lifeImage.canvas.transform.forward = Camera.main.transform.forward.normalized;
        }
        lifeImage.fillAmount = Mathf.Lerp(0, 1, (float)lifeSystem.GetCurrentLife/(float)lifeSystem.GetTotalLife);
    }
}
