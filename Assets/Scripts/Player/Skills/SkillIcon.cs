using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillIcon : MonoBehaviour
{
    [SerializeField] private Image bgImage;
    [SerializeField] private Image bgIcon;
    [SerializeField] private Image fillImage;
    [SerializeField] private Image fillIcon;

    public void UpdateFill(float maxSkillPoints, float currentSkillPoints)
    {
        var fillAmount = currentSkillPoints / maxSkillPoints;
        fillImage.fillAmount = fillAmount;
        fillIcon.fillAmount = fillAmount;
    }
}
