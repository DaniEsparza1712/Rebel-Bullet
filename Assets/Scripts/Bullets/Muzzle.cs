using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Muzzle : MonoBehaviour
{
    public float lifeTime;
    float timer;
    // Start is called before the first frame update
    void Start()
    {
        timer = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if(timer >= lifeTime){
            Destroy(gameObject);
        }
        timer += Time.deltaTime;
    }
}
