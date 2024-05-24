using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComponentInventory
{
    public static ComponentInventory Instance;
    public Dictionary<string, BarrelComponent> BarrelComponents = new();
    public Dictionary<string, MagazineComponent> MagazineComponents = new();
    public Dictionary<string, GripComponent> GripComponents = new();
    public Dictionary<string, MuzzleComponent> MuzzleComponents = new();
    public Dictionary<string, SpringComponent> SpringComponents = new();

    public void UpdateComponents(List<BarrelComponent> barrels, List<MagazineComponent> mags, List<GripComponent> grps, List<MuzzleComponent> muzzles, List<SpringComponent> springs)
    {
        BarrelComponents.Clear();
        foreach (var barrel in barrels)
        {
            BarrelComponents.Add(barrel.componentName, barrel);
        }
        MagazineComponents.Clear();
        foreach (var mag in mags)
        {
            MagazineComponents.Add(mag.componentName, mag);
        }
        GripComponents.Clear();
        foreach (var grp in grps)
        {
            GripComponents.Add(grp.componentName, grp);
        }
        MuzzleComponents.Clear();
        foreach (var muzzle in muzzles)
        {
            MuzzleComponents.Add(muzzle.componentName, muzzle);
        }
        SpringComponents.Clear();
        foreach (var spring in springs)
        {
            SpringComponents.Add(spring.componentName, spring);
        }
    }

    public void ChangeActiveBarrel(string key)
    {
        ComponentHolder.Instance.SetBarrel(BarrelComponents[key]);
    }
    public void ChangeActiveGrip(string key)
    {
        ComponentHolder.Instance.SetGrip(GripComponents[key]);
    }

    public void ChangeActiveMagazine(string key)
    {
        ComponentHolder.Instance.SetMagazine(MagazineComponents[key]);
    }
    public void ChangeActiveMuzzle(string key)
    {
        ComponentHolder.Instance.SetMuzzle(MuzzleComponents[key]);
    }

    public void ChangeActiveSpring(string key)
    {
        ComponentHolder.Instance.SetSprint(SpringComponents[key]);
    }
}
