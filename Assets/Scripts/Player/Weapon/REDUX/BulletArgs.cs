using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletArgs : EventArgs
{
    public Vector3 Dir;
    public Vector3 Pos;
    public Attack BulletAttack;
    public float Speed;

    public BulletArgs(Vector3 dir, Vector3 pos, Attack attack, float speed)
    {
        Dir = dir;
        Pos = pos;
        BulletAttack = attack;
        Speed = speed;
    }
}
