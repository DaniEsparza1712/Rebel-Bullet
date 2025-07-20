//Adapted from Gabriel Aguiar Prod.: https://www.youtube.com/watch?v=7vvycc2iX6E&t=428s

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerController))]
public class MeshTrail : MonoBehaviour
{
    private PlayerController _player;
    private SkinnedMeshRenderer[] _skinnedMeshRenderers;
    private bool _meshActive = false;
    [SerializeField] private float refreshRate;
    [SerializeField] private float destroyDelay;
    [SerializeField] private Material material;

    private void Awake()
    {
        _player = GetComponent<PlayerController>();
        _skinnedMeshRenderers = GetComponentsInChildren<SkinnedMeshRenderer>();

        _player.OnDashTrailStart += (sender, args) =>
        {
            _meshActive = true;
            StartCoroutine(TrailCoroutine());
        };

        _player.OnDashTrailEnd += (sender, args) =>
        {
            _meshActive = false;
        };
    }

    private IEnumerator TrailCoroutine()
    {
        while (_meshActive)
        {
            foreach (var skinnedMeshRenderer in _skinnedMeshRenderers)
            {
                var matsCount = skinnedMeshRenderer.materials.Length;
                var trailObj = new GameObject();
                trailObj.transform.position = transform.position;
                trailObj.transform.rotation = transform.rotation;
                
                var meshRenderer = trailObj.AddComponent<MeshRenderer>();
                var meshFilter = trailObj.AddComponent<MeshFilter>();
                var mesh = new Mesh();
                skinnedMeshRenderer.BakeMesh(mesh);
                meshFilter.mesh = mesh;

                List<Material> mats = new List<Material>();

                for (int i = 0; i < matsCount; i++)
                {
                    mats.Add(material);
                }
                meshRenderer.materials = mats.ToArray();
                
                Destroy(trailObj, destroyDelay);
            }
            yield return new WaitForSeconds(refreshRate);
        }
    }
}
