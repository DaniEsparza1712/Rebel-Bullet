using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NotifyArgs : EventArgs
{
    public Vector3 position;

    public NotifyArgs(Vector3 position)
    {
        this.position = position;
    }
}
