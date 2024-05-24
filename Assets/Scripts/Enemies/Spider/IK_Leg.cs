using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IK_Leg : MonoBehaviour
{
    public Transform body;
    public Transform legTarget;
    public IK_Leg[] opposites;
    public float rayPointY;
    public float lerpSpeed;
    float lerpCounter;
    public LayerMask floorMask;
    public float stepDistance;
    public float stepHeight;
    public float castDist;
    float distance;
    bool moving;
    Vector3 currentPos;
    private void Start() {
        currentPos = transform.position;
        moving = false;
    }
    // Update is called once per frame
    void Update()
    {
        transform.position = currentPos;
        Vector3 castPos = currentPos;
        castPos.y += rayPointY;
        Ray ray = new Ray(castPos, Vector3.down);
        distance = Vector3.Distance(legTarget.position, transform.position);

        if(distance >= stepDistance && !CheckOppositeMove())
            moving = true;

        if(Physics.Raycast(ray, out RaycastHit info, castDist, floorMask) && distance < stepDistance && !moving){
            lerpCounter = 0;
            transform.position = info.point;
        }
        else if(moving && !CheckOppositeMove()){
            lerpCounter += lerpSpeed * Time.deltaTime;
            currentPos = Vector3.Lerp(transform.position, legTarget.position, lerpCounter);
            currentPos.y += Mathf.Sin(lerpCounter * Mathf.PI) * stepHeight;
            if(lerpCounter >= 1){
                moving = false;
            }
        }
    }

    bool CheckOppositeMove(){
        foreach(IK_Leg leg in opposites){
            if(leg.moving)
                return true;
        }
        return false;
    }
}
