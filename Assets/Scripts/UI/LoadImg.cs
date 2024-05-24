using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Image = UnityEngine.UI.Image;
using Random = UnityEngine.Random;

public class LoadImg : MonoBehaviour
{
    [SerializeField] private List<Sprite> loadImages;
    public Image img;

    // Start is called before the first frame update
    private void OnEnable()
    {
        var index = Random.Range(0, loadImages.Count);
        var newImg = loadImages[index];
        img.sprite = newImg;
    }
}
