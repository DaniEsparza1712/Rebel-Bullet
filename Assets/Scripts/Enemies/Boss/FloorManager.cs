using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloorManager : MonoBehaviour
{
    [SerializeField] private Animator floorAnimator;
    [SerializeField] private float minWaitTime;
    [SerializeField] private float maxWaitTime;

    public void HideFloor()
    {
        StopCoroutine(HideFloorEnumerator());
        StartCoroutine(HideFloorEnumerator());
    }

    private IEnumerator HideFloorEnumerator()
    {
        floorAnimator.SetBool("Hide", true);
        yield return new WaitForSeconds(Random.Range(minWaitTime, maxWaitTime));
        floorAnimator.SetBool("Hide", false);
    }
}
