using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class DropOnDestroy : MonoBehaviour
{
    private EnemyHittable _hittable;
    [SerializeField] private GameObject droppable;
    [SerializeField] private Transform dropPoint;
    [SerializeField] private float delay;

    private void Awake()
    {
        _hittable = GetComponent<EnemyHittable>();
        _hittable.OnDeath += (sender, args) =>
        {
            StartCoroutine(Drop());
        };
    }

    private IEnumerator Drop()
    {
        yield return new WaitForSeconds(delay);
        Instantiate(droppable, dropPoint.position, dropPoint.rotation);
        gameObject.SetActive(false);
    }
}
