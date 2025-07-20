using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class WeaponPools : MonoBehaviour
{
    private Dictionary<string, BulletPool> _pools = new();
    [SerializeField] private Transform poolContainer;
    [SerializeField] private int poolSize = 7;

    public void CreatePools(GunCore[] cores)
    {
        _pools.Clear();
        foreach (var core in cores)
        {
            AddPool(core);
        }
    }

    public void AddPool(GunCore core)
    {
        var poolObject = Instantiate(new GameObject(), transform);
        var pool = poolObject.AddComponent<BulletPool>();
        pool.Setup(core.bulletPrefab);
        pool.FillPool(poolSize);
        _pools.Add(core.coreName, pool);
    }

    public BulletPool GetPool(string coreName)
    {
        return _pools[coreName];
    }
}
