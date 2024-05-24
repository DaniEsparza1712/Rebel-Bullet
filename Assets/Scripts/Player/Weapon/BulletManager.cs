using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class BulletManager : MonoBehaviour, ComponentInterface
{
    [Header("Game Objects")]
    public GameObject target;
    private GameObject _muzzle;
    
    private float _shootingRate;
    private int _bulletDamage;

    [Header("Masks")]
    public LayerMask shootingMask;

    [Header("External Components")]
    public OverheatSystem overheatSystem;
    public List<BulletPooler> poolers = new();
    private Dictionary<string, BulletPooler> _poolerDict = new();

    [Header("Camera")]
    public CinemachineImpulseSource impulseSource;
    
    private float _timer;
    private string _bullet;
    private bool _onOverheat;

    public void ChangeChildrenStats()
    {
        _shootingRate = 1 / ComponentHolder.Instance.ShootingRate;
        _bullet = ComponentHolder.Instance.GetBullet().name;
        _muzzle = ComponentHolder.Instance.GetMuzzle();
    }
    
    // Start is called before the first frame update
    void Start()
    {
        foreach (var pooler in poolers)
        {
            _poolerDict.Add(pooler.bulletKey, pooler);
        }
        _shootingRate = 1 / ComponentHolder.Instance.ShootingRate;
        _bullet = ComponentHolder.Instance.GetBullet().name;
        _muzzle = ComponentHolder.Instance.GetMuzzle();
        _timer = _shootingRate;
    }

    // Update is called once per frame
    void Update()
    {
        if((Input.GetButton("Fire1") || Input.GetAxisRaw("Fire1") == 1) && _timer >= _shootingRate && !overheatSystem.onOverheat && Time.timeScale > 0){
            var muzzleInst = Instantiate(_muzzle, transform.position, Quaternion.identity);
            muzzleInst.transform.SetParent(transform);

            var shootingPoint = GetShootingPoint(target.transform.position);
            muzzleInst.transform.forward = shootingPoint - muzzleInst.transform.position;
            
            var bulletInst = _poolerDict[_bullet].PullBullet(transform.position, shootingPoint - transform.position);
            impulseSource.GenerateImpulse();

            overheatSystem.AddOverheat();

            _timer = 0;
        }
        _timer += Time.deltaTime;
    }

    private Vector3 GetShootingPoint(Vector3 targetPos)
    {
        var accuracyMod = ComponentHolder.Instance.Accuracy;
        var point = targetPos;
        var xOffset = Random.Range(-5.0f, 5.0f) / accuracyMod;
        var yOffset = Random.Range(-5.0f, 5.0f) / accuracyMod;
        point += transform.right * xOffset + transform.up * yOffset;

        return point;
    }
}
