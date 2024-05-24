using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

[System.Serializable]
public class Preset
{
    public BarrelComponent barrel;
    public MagazineComponent magazine;
    public GripComponent grip;
    public MuzzleComponent muzzle;
    public SpringComponent spring;

    public Preset(BarrelComponent barr, MagazineComponent mag, GripComponent grp, 
        MuzzleComponent muz, SpringComponent spr)
    {
        barrel = barr;
        magazine = mag;
        grip = grp;
        muzzle = muz;
        spring = spr;
    }
    
    public void SetAsCurrent()
    {
        ComponentInventory.Instance.ChangeActiveBarrel(barrel.componentName);
        ComponentInventory.Instance.ChangeActiveGrip(grip.componentName);
        ComponentInventory.Instance.ChangeActiveMagazine(magazine.componentName);
        ComponentInventory.Instance.ChangeActiveMuzzle(muzzle.componentName);
        ComponentInventory.Instance.ChangeActiveSpring(spring.componentName);
    }
}