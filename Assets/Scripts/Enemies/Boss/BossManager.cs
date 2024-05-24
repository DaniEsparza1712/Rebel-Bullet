using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossManager : MonoBehaviour
{
    private LifeSystem _playerLife;
    // Start is called before the first frame update
    void Start()
    {
        _playerLife = GameObject.Find("Sam").GetComponent<LifeSystem>();
    }

    public void MakePlayerInvisible()
    {
        _playerLife.SetInvincible(true);
    }
}
