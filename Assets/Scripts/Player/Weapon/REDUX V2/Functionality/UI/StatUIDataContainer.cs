using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class StatUIDataContainer : MonoBehaviour
{
    [SerializeField] private TMP_Text statNameText;
    [SerializeField] private TMP_Text statValueText;

    public void SetStatName(string statName)
    {
        statNameText.text = statName;
    }

    public void SetStatValue(string statValue)
    {
        statValueText.text = statValue;
    }
}
