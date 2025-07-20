using System;
using System.Collections;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(WeaponPools))]
[RequireComponent(typeof(BulletShooter))]
[RequireComponent(typeof(PlayerController))]
public class WeaponManager : MonoBehaviour
{
    [SerializeField] private GunBody defaultBodyData;
    [SerializeField] private GunBarrel defaultBarrelData;
    [SerializeField] private GunGrip defaultGripData;
    [SerializeField] private GunStock defaultStockData;
    [SerializeField] private GunCore defaultCoreData;

    [SerializeField] private WeaponOptions weaponOptions;
    private WeaponStats _currentStats;

    [SerializeField] private Transform weaponContainer;
    public WeaponStats CurrentStats => _currentStats;
    private WeaponPools _pools;
    private BulletShooter _bulletShooter;
    
    private GunBody _bodyData;
    private GunBarrel _barrelData;
    private GunGrip _gripData;
    private GunStock _stockData;
    private GunCore _coreData;
    
    private GameObject _currentBody;
    private GameObject _currentBarrel;
    private GameObject _currentGrip;
    private GameObject _currentStock;
    
    private PlayerController _playerController;
    
    [SerializeField] private Material highlightMaterial;

    public EventHandler OnChangeComponent;

    private void Awake()
    {
        weaponOptions.GenerateDictionaries();
        BuildDefaultGun();
        _currentStats = new WeaponStats(_bodyData, _barrelData, _gripData, _stockData, _coreData);
        
        _pools = GetComponent<WeaponPools>();
        _pools.CreatePools(weaponOptions.GetCores());
        
        _bulletShooter = GetComponent<BulletShooter>();
        _bulletShooter.SetPool(_pools.GetPool(_coreData.coreName));
        _bulletShooter.SetStats(_currentStats.CurrentAttack, _currentStats.ProjectileSpeed, _currentStats.FireRate);
        
        highlightMaterial.color = _coreData.elementColor;
        OnChangeComponent += (sender, args) =>
        {
            _currentStats.UpdateStats(_bodyData, _barrelData, _gripData, _stockData, _coreData);
            highlightMaterial.color = _coreData.elementColor;
            
            _bulletShooter.SetPool(_pools.GetPool(_coreData.coreName));
            _bulletShooter.SetStats(_currentStats.CurrentAttack, _currentStats.ProjectileSpeed, _currentStats.FireRate);
        };
        
        //Set player shooting events
        _playerController = GetComponent<PlayerController>();
        _playerController.OnShootStarted += (sender, args) =>
        {
            _bulletShooter.StartShooting();
        };
        _playerController.OnShootEnded += (sender, args) =>
        {
            _bulletShooter.StopShooting();
        };
    }

    //Build data and object
    public void BuildDefaultGun()
    {
        _bodyData = defaultBodyData;
        _barrelData = defaultBarrelData;
        _gripData = defaultGripData;
        _stockData = defaultStockData;
        _coreData = defaultCoreData;
        
        _currentBody = Instantiate(_bodyData.bodyPrefab, weaponContainer);
        _currentBarrel = Instantiate(_barrelData.barrelPrefab, weaponContainer);
        _currentGrip = Instantiate(_gripData.grip, weaponContainer);
        _currentStock = Instantiate(_stockData.stock, weaponContainer);
        
        PositionComponents();
    }

    //Set offsets according to body
    private void PositionComponents()
    {
        _currentBarrel.transform.localPosition = _bodyData.barrelPosition;
        _currentGrip.transform.localPosition = _bodyData.gripPosition;
        _currentStock.transform.localPosition = _bodyData.stockPosition;
    }
    
    //Change body component through body data
    public void ChangeBody(GunBody newBodyData)
    {
        _bodyData = newBodyData;
        Destroy(_currentBody);
        _currentBody = Instantiate(_bodyData.bodyPrefab, weaponContainer);
        PositionComponents();
        OnChangeComponent?.Invoke(this, EventArgs.Empty);
    }

    //Change body component through name: check weaponOptions dictionary
    public void ChangeBody(string newBodyName)
    {
        _bodyData = weaponOptions.GetBody(newBodyName);
        Destroy(_currentBody);
        _currentBody = Instantiate(_bodyData.bodyPrefab, weaponContainer);
        PositionComponents();
        OnChangeComponent?.Invoke(this, EventArgs.Empty);
    }

    public void ChangeBarrel(GunBarrel newBarrelData)
    {
        _barrelData = newBarrelData;
        Destroy(_currentBarrel);
        _currentBarrel = Instantiate(_barrelData.barrelPrefab, weaponContainer);
        PositionComponents();
        OnChangeComponent?.Invoke(this, EventArgs.Empty);
    }

    //Change barrel component through name: check weaponOptions dictionary
    public void ChangeBarrel(string newBarrelName)
    {
        _barrelData = weaponOptions.GetBarrel(newBarrelName);
        Destroy(_currentBarrel);
        _currentBarrel = Instantiate(_barrelData.barrelPrefab, weaponContainer);
        PositionComponents();
        OnChangeComponent?.Invoke(this, EventArgs.Empty);
    }

    public void ChangeGrip(GunGrip newGripData)
    {
        _gripData = newGripData;
        Destroy(_currentGrip);
        _currentGrip = Instantiate(_gripData.grip, weaponContainer);
        PositionComponents();
        OnChangeComponent?.Invoke(this, EventArgs.Empty);
    }

    //Change grip component through name: check weaponOptions dictionary
    public void ChangeGrip(string newGripName)
    {
        _gripData = weaponOptions.GetGrip(newGripName);
        Destroy(_currentGrip);
        _currentGrip = Instantiate(_gripData.grip, weaponContainer);
        PositionComponents();
        OnChangeComponent?.Invoke(this, EventArgs.Empty);
    }

    public void ChangeStock(GunStock newStockData)
    {
        _stockData = newStockData;
        Destroy(_currentStock);
        _currentStock = Instantiate(_stockData.stock, weaponContainer);
        PositionComponents();
        OnChangeComponent?.Invoke(this, EventArgs.Empty);
    }
    
    //Change core component through name: check weaponOptions dictionary
    public void ChangeCore(string newCoreName)
    {
        _coreData = weaponOptions.GetCore(newCoreName);
        OnChangeComponent?.Invoke(this, EventArgs.Empty);
    }
    
    public void ChangeCore(GunCore newCoreData)
    {
        _coreData = newCoreData;
        OnChangeComponent?.Invoke(this, EventArgs.Empty);
    }

    //Change core component through name: check weaponOptions dictionary
    public void ChangeStock(string newStockName)
    {
        _stockData = weaponOptions.GetStock(newStockName);
        Destroy(_currentStock);
        _currentStock = Instantiate(_stockData.stock, weaponContainer);
        PositionComponents();
        OnChangeComponent?.Invoke(this, EventArgs.Empty);
    }

    //Functions for getting a list by name of each component type
    public string[] GetBodyNames()
    {
        return weaponOptions.GetBodies().Select(body => body.bodyName).ToArray();
    }

    public string[] GetBarrelNames()
    {
        return weaponOptions.GetBarrels().Select(barrel => barrel.barrelName).ToArray();
    }

    public string[] GetGripNames()
    {
        return weaponOptions.GetGrips().Select(grip => grip.gripName).ToArray();
    }

    public string[] GetStockNames()
    {
        return weaponOptions.GetStocks().Select(stock => stock.stockName).ToArray();
    }

    public string[] GetCoreNames()
    {
        return weaponOptions.GetCores().Select(core => core.coreName).ToArray();
    }
}
