using System;
using UnityEngine;

public class AttackArgs : EventArgs
{
    public Attack attack {get;}
    public Vector3 hitPoint {get;}
    public Vector3 hitDirection {get;}
    public Vector3 hitOrigin { get; }

    public AttackArgs(Attack attack, Vector3 hitPoint, Vector3 hitDirection, Vector3 hitOrigin)
    {
        this.attack = attack;
        this.hitPoint = hitPoint;
        this.hitDirection = hitDirection;
        this.hitOrigin = hitOrigin;
    }
}
