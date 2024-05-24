using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IK_Target : MonoBehaviour
{
    public Transform body;
    public LayerMask floorMask;
    public float rayPointY;
    // Update is called once per frame
    void Update()
    {
        Vector3 castPos = transform.position;
        castPos.y = rayPointY;
        Ray ray = new Ray(castPos, Vector3.down);
        if(Physics.Raycast(ray, out RaycastHit info, 0.3f, floorMask)){
            transform.position = info.point;
        }
    }
}
