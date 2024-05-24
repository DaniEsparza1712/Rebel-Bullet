using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VendingMachine : MonoBehaviour
{
    private LifeSystem _playerLife;
    [SerializeField] private int lifePoints;

    private void Start()
    {
        _playerLife = GameObject.Find("Sam").GetComponent<LifeSystem>();
    }

    public void AddLife()
    {
        _playerLife.AddLife(lifePoints);
    }
}
