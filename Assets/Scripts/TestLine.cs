using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.ProBuilder;

public class TestLine : MonoBehaviour
{
    public Vector3 viewDir;
    public CharacterController character;
    private void OnDrawGizmos() {
        Gizmos.color = Color.green;
        Gizmos.DrawRay(transform.position, viewDir);
        Gizmos.color = new Color(0, 1, 1, 0.5f);
        Gizmos.DrawSphere(transform.position + transform.up * character.radius, character.radius);
        //Gizmos.DrawCube(transform.position + new Vector3(-1.5f, 1.5f, 3), new Vector3(3, 3, 6));
    }
}
