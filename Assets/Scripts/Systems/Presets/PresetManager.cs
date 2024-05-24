using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class PresetManager : MonoBehaviour
{
    [SerializeField] 
    private Preset defaultPreset;
    [SerializeField] private PresetUI presetUI;
    [SerializeField] private TMP_Text presetText;
    public SliderContainer sliderContainer;

    [SerializeField]
    private int presetsSize;
    private int _currentIndex;
    private readonly List<Preset> _presets = new();
    
    
    // Start is called before the first frame update
    private void Start()
    {
        ComponentHolder.Instance.GetStats();
        for (int i = 0; i < presetsSize; i++)
        {
            _presets.Add(new Preset(defaultPreset.barrel, defaultPreset.magazine, defaultPreset.grip, 
                defaultPreset.muzzle, defaultPreset.spring));
            var buttonPref = Instantiate(presetUI.ButtonPref, presetUI.content);
            buttonPref.GetComponent<PresetButton>().SetComps(presetUI.barrelTMP, presetUI.grpTMP, 
                presetUI.magTMP, presetUI.muzTMP, presetUI.sprTMP, i, this);
            buttonPref.GetComponent<Button>().onClick.AddListener(ChangeSliders);
        }
    }

    private void Update()
    {
        if (Input.GetButtonDown("PresetNext"))
            ChangeEquippedPreset(1);
        else if (Input.GetButtonDown("PresetPrev"))
            ChangeEquippedPreset(-1);
    }

    public void ChangePreset(int index, Preset preset)
    {
        _presets[index] = preset;
        ChangedEquippedPresetByIndex(index);
    }

    public void ChangeEquippedPreset(int next)
    {
        next = Math.Clamp(next, -1, 1);
        _currentIndex += next;
        if (_currentIndex < 0)
            _currentIndex = presetsSize - 1;
        else if (_currentIndex > presetsSize - 1)
            _currentIndex = 0;
        _presets[_currentIndex].SetAsCurrent();
        presetText.text = "Preset " + (_currentIndex + 1);
        ExecuteEvents.Execute<ComponentInterface>(gameObject, null, (x, y) => x.ChangeChildrenStats());
    }

    public void ChangedEquippedPresetByIndex(int index)
    {
        index = Math.Clamp(index, 0, presetsSize - 1);
        _presets[index].SetAsCurrent();
        _currentIndex = index;
        presetText.text = "Preset " + (_currentIndex + 1);
        ExecuteEvents.Execute<ComponentInterface>(gameObject, null, (x, y) => x.ChangeChildrenStats());
    }

    public void ChangeSliders()
    {
        ComponentHolder.Instance.GetStats();
        var holder = ComponentHolder.Instance;
        
        sliderContainer.rateSlider.UpdateUI(holder.ShootingRate);
        sliderContainer.damageSlider.UpdateUI(holder.Damage);
        sliderContainer.speedSlider.UpdateUI(holder.BulletSpeed);
        sliderContainer.sizeSlider.UpdateUI(holder.BulletSize);
        sliderContainer.accuracySlider.UpdateUI(holder.Accuracy);
        sliderContainer.overheatSlider.UpdateUI(holder.OverheatRate);
        sliderContainer.lifetimeSlider.UpdateUI(holder.BulletLife);
    }

    [Serializable]
    public class SliderContainer
    {
        public Update rateSlider;
        public Update damageSlider;
        public Update speedSlider;
        public Update sizeSlider;
        public Update accuracySlider;
        public Update overheatSlider;
        public Update lifetimeSlider;
    }
}
