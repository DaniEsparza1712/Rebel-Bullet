using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallManager : MonoBehaviour
{
    [SerializeField] private List<GameObject> walls;
    [SerializeField] private Animator animator;

    public void SetWallsActive()
    {
        foreach (var wall in walls)
        {
            wall.SetActive(true);
        }
        animator.SetBool("Hidden", false);
    }
    public void SetWallsInactive()
    {
        animator.SetBool("Hidden", true);
    }
}
