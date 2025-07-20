using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunPartsManager : MonoBehaviour
{
    [SerializeField] private List<ChipElement> chipElements;
    [SerializeField] private List<ChipStyle> chipStyles;
    
    [SerializeField] private ChipElement selectedChipElement;
    [SerializeField] private ChipStyle selectedChipStyle;
    
    public ChipStyle SelectedChipStyle => selectedChipStyle;
}
