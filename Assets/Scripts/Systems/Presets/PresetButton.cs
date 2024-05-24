using TMPro;
using UnityEngine;

public class PresetButton : MonoBehaviour
{
    private TMP_Text _barrelKey;
    private TMP_Text _gripKey;
    private TMP_Text _magazineKey;
    private TMP_Text _muzzleKey;
    private TMP_Text _springKey;

    private int _presetIndex;
    private PresetManager _presetManager;

    public void SetComps(TMP_Text bKey, TMP_Text gKey, TMP_Text maKey, TMP_Text muKey, TMP_Text sKey, int index, PresetManager manager)
    {
        _barrelKey = bKey;
        _gripKey = gKey;
        _magazineKey = maKey;
        _muzzleKey = muKey;
        _springKey = sKey;

        _presetIndex = index;
        _presetManager = manager;

        GetComponentInChildren<TMP_Text>().text = "Save and equip preset " + (index + 1);
    }

    public void SetPreset()
    {
        ComponentInventory instance = ComponentInventory.Instance;
        
        BarrelComponent barr = instance.BarrelComponents[_barrelKey.text];
        GripComponent grp = instance.GripComponents[_gripKey.text];
        MagazineComponent mag = instance.MagazineComponents[_magazineKey.text];
        MuzzleComponent muz = instance.MuzzleComponents[_muzzleKey.text];
        SpringComponent spr = instance.SpringComponents[_springKey.text];

        Preset newPreset = new Preset(barr, mag, grp, muz, spr);
        _presetManager.ChangePreset(_presetIndex, newPreset);
    }
}
