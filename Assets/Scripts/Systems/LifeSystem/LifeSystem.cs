using System.Collections;
using System.Collections.Generic;
using System.Data.SqlTypes;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.ProBuilder;

public class LifeSystem : MonoBehaviour
{
    [Header("Life")]
    [SerializeField]
    public int totalLife;
    public int GetTotalLife => totalLife;
    [SerializeField]
    private int currentLife;
    public int GetCurrentLife => currentLife;

    [Header("Regeneration")]
    [SerializeField]
    private float regenerationRate;
    [SerializeField]
    private int regenerationValue;

    private bool _invincible;
    private float timer;

    [Header("Events")]
    public UnityEvent OnDeath;
    bool calledEvent;

    // Start is called before the first frame update
    void Start()
    {
        calledEvent = false;
        currentLife = totalLife;
        _invincible = false;
        timer = 0;
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        if(timer >= regenerationRate && !calledEvent)
        {
            currentLife = Mathf.Clamp(currentLife + regenerationValue, 0, totalLife);
            timer = 0;
        }
        if(currentLife <= 0 && !calledEvent)
        {
            calledEvent = true;
            OnDeath.Invoke();
        }
    }

    public void ApplyDamage(int damage)
    {
        if(!_invincible)
        {
            int newLife = currentLife - Mathf.Abs(damage);
            currentLife = Mathf.Clamp(newLife, 0, totalLife);
            StartCoroutine(MaterialColorOnImpact(0.1f, Color.red));
        }
    }

    public void AddLife(int lifePoints)
    {
        currentLife = Mathf.Min(totalLife, currentLife + lifePoints);
}

    List<Material> GetMaterials()
    {
        MeshRenderer[] renderers = GetComponentsInChildren<MeshRenderer>();
        SkinnedMeshRenderer[] skinnedRenderers = GetComponentsInChildren<SkinnedMeshRenderer>();
        List<Material> mats = new List<Material>();

        foreach (MeshRenderer renderer in renderers)
        {
            foreach (Material mat in renderer.materials)
            {
                if(!mat.name.Contains("ND"))
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

    public void SetInvincible(bool invincible)
    {
        _invincible = invincible;
    }

    IEnumerator MaterialColorOnImpact(float waitTime, Color damageColor)
    {
        if (!calledEvent)
        {
            List<Material> mats = GetMaterials();

            foreach (Material mat in mats)
            {
                mat.color = damageColor;
            }

            yield return new WaitForSeconds(waitTime);

            foreach (Material material in mats)
            {
                material.color = Color.white;
            }
        }
    }

    public void SetRegen(float rate, int val)
    {
        regenerationRate = rate;
        regenerationValue = val;
    }

    //Standard Death Functions
    IEnumerator TurnBlackOverTime()
    {
        float lerpVal = 0;
        float lerpSpeed = 0.1f;
        List<Material> mats = GetMaterials();

        while (lerpVal <= 1)
        {
            foreach(Material mat in mats)
            {
                mat.color = Color.Lerp(Color.white, Color.black, lerpVal);
            }
            lerpVal += lerpSpeed;
            yield return new WaitForSeconds(lerpSpeed);
        }
    }
    IEnumerator WaitForDestroy(float time)
    {
        yield return new WaitForSeconds(time);
        Destroy(gameObject);
    }
    public void CallBlackOverTime()
    {
        StartCoroutine(TurnBlackOverTime());
    }
    public void DestroyAfterTime(float time)
    {
        StartCoroutine(WaitForDestroy(time));
    }
}
