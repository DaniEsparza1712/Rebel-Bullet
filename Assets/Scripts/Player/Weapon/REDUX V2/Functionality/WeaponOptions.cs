using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[Serializable]
public class WeaponOptions
{
    [Header("Options")]
    [SerializeField] private List<GunBarrel> barrels;
    [SerializeField] private List<GunBody> bodies;
    [SerializeField] private List<GunGrip> grips;
    [SerializeField] private List<GunStock> stocks;
    [SerializeField] private List<GunCore> cores;
    
    private Dictionary<string, GunBarrel> _barrelOptions = new();
    private Dictionary<string, GunBody> _bodyOptions = new();
    private Dictionary<string, GunGrip> _gripOptions = new();
    private Dictionary<string, GunStock> _stockOptions = new();
    private Dictionary<string, GunCore> _coresOptions = new();

    public void GenerateDictionaries()
    {
        foreach (var barrel in barrels)
        {
            _barrelOptions.Add(barrel.barrelName, barrel);
        }

        foreach (var body in bodies)
        {
            _bodyOptions.Add(body.bodyName, body);
        }

        foreach (var grip in grips)
        {
            _gripOptions.Add(grip.gripName, grip);
        }

        foreach (var stock in stocks)
        {
            _stockOptions.Add(stock.stockName, stock);
        }

        foreach (var core in cores)
        {
            _coresOptions.Add(core.coreName, core);
        }
    }

    public GunBarrel[] GetBarrels()
    {
        return _barrelOptions.Values.ToArray();
    }

    public GunBody[] GetBodies()
    {
        return _bodyOptions.Values.ToArray();
    }

    public GunGrip[] GetGrips()
    {
        return _gripOptions.Values.ToArray();
    }

    public GunStock[] GetStocks()
    {
        return _stockOptions.Values.ToArray();
    }

    public GunCore[] GetCores()
    {
        return _coresOptions.Values.ToArray();
    }

    public GunBody GetBody(string bodyName)
    {
        return _bodyOptions[bodyName];
    }

    public GunBarrel GetBarrel(string barrelName)
    {
        return _barrelOptions[barrelName];
    }

    public GunGrip GetGrip(string gripName)
    {
        return _gripOptions[gripName];
    }

    public GunStock GetStock(string stockName)
    {
        return _stockOptions[stockName];
    }
    
    public GunCore GetCore(string coreName)
    {
        return _coresOptions[coreName];
    }
}
