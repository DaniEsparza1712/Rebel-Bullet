using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ComponentInitializer : MonoBehaviour
{
    [Header("Components")]
    public List<BarrelComponent> barrels;
    public List<MagazineComponent> magazines;
    public List<GripComponent> grips;
    public List<MuzzleComponent> muzzles;
    public List<SpringComponent> springs;
    
    [Header("Current Component")]
    public BarrelComponent barrel;
    public GripComponent grip;
    public MagazineComponent magazine;
    public MuzzleComponent muzzle;
    public SpringComponent spring;
    private void Awake()
    {
        ComponentInventory.Instance = new ComponentInventory();
        ComponentInventory.Instance.UpdateComponents(barrels, magazines, grips, muzzles, springs);
        
        ComponentHolder.Instance = new ComponentHolder(barrel, grip, magazine, muzzle, spring);
        ComponentHolder.Instance.GetStats();
    }

    public void AddComponent(WeaponComponent component)
    {
        switch (component)
        {
            case BarrelComponent barrelComponent:
                if(!barrels.Contains(barrelComponent))
                    barrels.Add(barrelComponent);
                break;
            case GripComponent gripComponent:
                if(!grips.Contains(gripComponent))
                    grips.Add(gripComponent);
                break;
            case MagazineComponent magazineComponent:
                if(!magazines.Contains(magazineComponent))
                    magazines.Add(magazineComponent);
                break;
            case MuzzleComponent muzzleComponent:
                if(!muzzles.Contains(muzzleComponent))
                    muzzles.Add(muzzleComponent);
                break;
            case SpringComponent springComponent:
                if(!springs.Contains(springComponent))
                    springs.Add(springComponent);
                break;
        }
        ComponentInventory.Instance.UpdateComponents(barrels, magazines, grips, muzzles, springs);
    }

    public void ChangeBarrel(string currentBarrel)
    {
        ComponentInventory.Instance.ChangeActiveBarrel(currentBarrel);
        ComponentHolder.Instance.GetStats();
        ExecuteEvents.Execute<ComponentInterface>(gameObject, null, (x, y) => x.ChangeChildrenStats());
    }
    
    public void ChangeGrip(string currentGrip)
    {
        ComponentInventory.Instance.ChangeActiveGrip(currentGrip);
        ComponentHolder.Instance.GetStats();
        ExecuteEvents.Execute<ComponentInterface>(gameObject, null, (x, y) => x.ChangeChildrenStats());
    }
    
    public void ChangeMagazine(string currentMag)
    {
        ComponentInventory.Instance.ChangeActiveMagazine(currentMag);
        ComponentHolder.Instance.GetStats();
        ExecuteEvents.Execute<ComponentInterface>(gameObject, null, (x, y) => x.ChangeChildrenStats());
    }
    
    public void ChangeMuzzle(string currentMuz)
    {
        ComponentInventory.Instance.ChangeActiveMuzzle(currentMuz);
        ComponentHolder.Instance.GetStats();
        ExecuteEvents.Execute<ComponentInterface>(gameObject, null, (x, y) => x.ChangeChildrenStats());
    }

    public void ChangeSpring(string currentSpr)
    {
        ComponentInventory.Instance.ChangeActiveSpring(currentSpr);
        ComponentHolder.Instance.GetStats();
        ExecuteEvents.Execute<ComponentInterface>(gameObject, null, (x, y) => x.ChangeChildrenStats());
    }
}
