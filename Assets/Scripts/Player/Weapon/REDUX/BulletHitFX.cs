using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletHitFX : MonoBehaviour
{
    [SerializeField] private BulletController parent;

    private void OnParticleSystemStopped()
    {
        parent.OnDisappear?.Invoke(this, EventArgs.Empty);
    }
}
