using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Body : MonoBehaviour
{
    public Transform[] leftLegs;
    public Transform[] rightLegs;
    public float offset;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.rotation = Quaternion.Euler(0, 0, GetAngle());
    }

    Vector3 GetAverageLegsPos(Transform[] legs){
        float x = 0;
        float y = 0;
        float z = 0;

        foreach(Transform leg in legs){
            x += leg.position.x;
            y += leg.position.y;
            z += leg.position.z;
        }

        x /= legs.Length;
        y /= legs.Length;
        z /= legs.Length;

        return new Vector3(x, y, z);
    }

    float LegWidth(){
        return Vector3.Distance(GetAverageLegsPos(leftLegs), GetAverageLegsPos(rightLegs));
    }

    float HeightDifference(){
        return GetAverageLegsPos(rightLegs).y - GetAverageLegsPos(leftLegs).y;
    }

    float GetAngle(){
        return -HeightDifference() * 90 / LegWidth();
    }
}
