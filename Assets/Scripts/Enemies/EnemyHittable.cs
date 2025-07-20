using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class EnemyHittable : MonoBehaviour
{
    [SerializeField]
    private Material material;
    [SerializeField] private float colorTime = 0.3f;
    [SerializeField] private int lifePoints = 5;
    private int _currentLife;
    public int GetLifePoints => lifePoints;
    public int GetCurrentLife => _currentLife;
    [SerializeField] private bool changeMat = true;
    private Animator _animator;
    private SkinnedMeshRenderer[] _skinnedMeshRenderers;
    private MeshRenderer[] _meshRenderer;
    
    private Dictionary<SkinnedMeshRenderer, Material> _skinnedMaterials = new Dictionary<SkinnedMeshRenderer, Material>();
    private Dictionary<MeshRenderer, Material> _meshMaterials = new Dictionary<MeshRenderer, Material>();
    private bool _death = false;

    public EventHandler<AttackArgs> OnHit;
    public EventHandler<AttackArgs> OnDeath;

    private void Awake()
    {
        _currentLife = lifePoints;
        _skinnedMeshRenderers = GetComponentsInChildren<SkinnedMeshRenderer>();
        _meshRenderer = GetComponentsInChildren<MeshRenderer>();
        _animator = GetComponent<Animator>();
        foreach (var rend in _skinnedMeshRenderers)
        {
            _skinnedMaterials.Add(rend, rend.material);
        }

        foreach (var rend in _meshRenderer)
        {
            _meshMaterials.Add(rend, rend.material);
        }
    }

    public void OnGetHit(AttackArgs args)
    {
        if (_death)
            return;
        _currentLife -= Mathf.Abs((int) args.attack.damage);
        _death = _currentLife <= 0;
        if (_death)
            OnDeath?.Invoke(this, args);
        else
            OnHit?.Invoke(this, args);
        
        if(changeMat)
            StartCoroutine(ChangeMat());
    }

    private IEnumerator ChangeMat()
    {
        foreach (var skinned in _skinnedMaterials.Keys)
        {
            skinned.material = material;
        }

        foreach (var mesh in _meshRenderer)
        {
            mesh.material = material;
        }
        yield return new WaitForSeconds(colorTime);
        foreach (var skinned in _skinnedMaterials.Keys)
        {
            skinned.material = _skinnedMaterials[skinned];
        }

        foreach (var mesh in _meshRenderer)
        {
            mesh.material = _meshMaterials[mesh];
        }
    }
}
