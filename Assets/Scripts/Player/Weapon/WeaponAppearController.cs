using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class WeaponAppearController : MonoBehaviour
{
    [SerializeField]
    private List<Material> mats;
    [SerializeField]
    private float speed;
    private float lerpValue;
    private bool active;
    // Start is called before the first frame update
    void Start()
    {
        active = false;
        lerpValue = 1;
        mats = GetMats();
        foreach(Material mat in mats)
        {
            mat.SetFloat("_Vanish", lerpValue);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(active && lerpValue > 0)
        {
            lerpValue -= Mathf.Abs(speed) * Time.deltaTime;
            foreach (Material mat in mats)
            {
                mat.SetFloat("_Vanish", lerpValue);
            }
        }
        else if(!active && lerpValue < 1)
        {
            lerpValue += Mathf.Abs(speed) * Time.deltaTime;
            foreach (Material mat in mats)
            {
                mat.SetFloat("_Vanish", lerpValue);
            }
        }
    }
    public void Appear()
    {
        active = true;
    }
    public void Disappear()
    {
        active = false;
    }

    List<Material> GetMats()
    {
        MeshRenderer[] renderers = GetComponentsInChildren<MeshRenderer>();
        SkinnedMeshRenderer[] skinnedRenderers = GetComponentsInChildren<SkinnedMeshRenderer>();
        List<Material> mats = new List<Material>();

        foreach (MeshRenderer renderer in renderers)
        {
            foreach (Material mat in renderer.materials)
            {
                mats.Add(mat);
            }
        }
        foreach (SkinnedMeshRenderer skinnedRenderer in skinnedRenderers)
        {
            foreach (Material mat in skinnedRenderer.materials)
            {
                if (!mat.name.Contains("ND"))
                    mats.Add(mat);
            }
        }
        return mats;
    }
}
