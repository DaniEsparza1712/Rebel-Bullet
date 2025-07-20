using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class WeaponDropdown : MonoBehaviour
{
    [SerializeField] private WeaponManager weaponManager;
    [SerializeField] private TMP_Dropdown bodyDropdown;
    [SerializeField] private TMP_Dropdown barrelDropdown;
    [SerializeField] private TMP_Dropdown gripDropdown;
    [SerializeField] private TMP_Dropdown stockDropdown;
    [SerializeField] private TMP_Dropdown coreDropdown;

    private void Start()
    {
        PopulateOptions();
        AddListenersToDropdown();
    }

    private void PopulateOptions()
    {
        bodyDropdown.ClearOptions();
        bodyDropdown.AddOptions(weaponManager.GetBodyNames().ToList());
        
        barrelDropdown.ClearOptions();
        barrelDropdown.AddOptions(weaponManager.GetBarrelNames().ToList());
        
        gripDropdown.ClearOptions();
        gripDropdown.AddOptions(weaponManager.GetGripNames().ToList());
        
        stockDropdown.ClearOptions();
        stockDropdown.AddOptions(weaponManager.GetStockNames().ToList());
        
        coreDropdown.ClearOptions();
        coreDropdown.AddOptions(weaponManager.GetCoreNames().ToList());
    }

    private void AddListenersToDropdown()
    {
        bodyDropdown.onValueChanged.AddListener(_ =>
        {
            weaponManager.ChangeBody(bodyDropdown.options[bodyDropdown.value].text);
        });
        barrelDropdown.onValueChanged.AddListener(_ =>
        {
            weaponManager.ChangeBarrel(barrelDropdown.options[barrelDropdown.value].text);
        });
        gripDropdown.onValueChanged.AddListener(_ =>
        {
            weaponManager.ChangeGrip(gripDropdown.options[gripDropdown.value].text);
        });
        stockDropdown.onValueChanged.AddListener(_ =>
        {
            weaponManager.ChangeStock(stockDropdown.options[stockDropdown.value].text);
        });
        coreDropdown.onValueChanged.AddListener(_ =>
        {
            weaponManager.ChangeCore(coreDropdown.options[coreDropdown.value].text);
        });
    }
}
