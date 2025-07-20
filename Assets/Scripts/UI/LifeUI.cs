using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LifeUI : MonoBehaviour
{
    [SerializeField] private Image lifeBar;
    [SerializeField] private Image damageBar;
    [Range(0.0f, 1.0f)][SerializeField] private float lifeBarSpeed;
    [SerializeField] private float lifeBarRate;
    [Range(0.0f, 1.0f)][SerializeField] private float damageBarSpeed;
    [SerializeField] private float damageBarRate;
    [SerializeField] private float waitTimeForDamageAnim;
    [SerializeField] private EnemyHittable hittable;

    private void Awake()
    {
        hittable.OnHit += (sender, args) =>
        {
            StopAllCoroutines();
            if (lifeBar)
                StartCoroutine("LifeBarAnim");
        };
        hittable.OnDeath += (sender, args) =>
        {
            gameObject.SetActive(false);
        };
    }

    private IEnumerator LifeBarAnim()
    {
        var targetFill = (float)hittable.GetCurrentLife / (float)hittable.GetLifePoints;
        var currentFill = lifeBar.fillAmount;
        var fillLerp = 0.0f;
        while (fillLerp < 1)
        {
            lifeBar.fillAmount = Mathf.Lerp(currentFill, targetFill, fillLerp);
            fillLerp = Mathf.Clamp(fillLerp + lifeBarSpeed, 0.0f, 1.0f);
            yield return new WaitForSeconds(lifeBarRate);
        }

        if (damageBar)
        {
            yield return new WaitForSeconds(waitTimeForDamageAnim);
            StartCoroutine("DamageBarAnim");
        }
    }
    
    private IEnumerator DamageBarAnim()
    {
        var targetFill = (float)hittable.GetCurrentLife / (float)hittable.GetLifePoints;
        var currentFill = damageBar.fillAmount;
        var fillLerp = 0.0f;
        while (fillLerp < 1)
        {
            damageBar.fillAmount = Mathf.Lerp(currentFill, targetFill, fillLerp);
            fillLerp = Mathf.Clamp(fillLerp + damageBarSpeed, 0.0f, 1.0f);
            yield return new WaitForSeconds(damageBarRate);
        }
    }
}
