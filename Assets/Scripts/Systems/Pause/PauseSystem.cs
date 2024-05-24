using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class PauseSystem : MonoBehaviour
{
    private bool _onPause;
    public TMP_Dropdown barrelsDropdown;
    public TMP_Dropdown magazineDropdown;
    public TMP_Dropdown gripDropdown;
    public TMP_Dropdown muzzleDropdown;
    public TMP_Dropdown springDropdown;
    [SerializeField]
    private RectTransform pauseScreen;
    private PresetManager _presetManager;
    
    public Camera pauseCam;
    private Camera _mainCam;
    private float _nearPane;

    private Manager _manager;

    private List<Canvas> _canvasList = new();
    // Start is called before the first frame update
    void Start()
    {
        _mainCam = Camera.main;

        pauseCam.enabled = false;
        _presetManager = GameObject.FindGameObjectWithTag("Player").GetComponentInChildren<PresetManager>();
        _manager = GetComponent<Manager>();
        _onPause = false;
        Cursor.lockState = CursorLockMode.Locked;
        pauseScreen.gameObject.SetActive(false);
        SetComponents();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Pause") && _manager.hasControl)
        {
            _onPause = !_onPause;
            if (_onPause)
            {
                _nearPane = _mainCam.nearClipPlane;
                _mainCam.nearClipPlane = 0;
                pauseCam.enabled = true;
                
                Time.timeScale = 0;
                pauseScreen.gameObject.SetActive(true);
                Cursor.lockState = CursorLockMode.Confined;
                SetComponents();
                GetCanvasList();
                _presetManager.ChangeSliders();
                ChangeActiveCanvas(false);
            }
            else
            {
                _mainCam.nearClipPlane = _nearPane;
                pauseCam.enabled = false;
                
                Time.timeScale = 1;
                pauseScreen.gameObject.SetActive(false);
                Cursor.lockState = CursorLockMode.Locked;
                ChangeActiveCanvas(true);
            }
        }
    }

    void SetComponents()
    {
        SetBarrels();
        SetMagazines();
        SetGrips();
        SetMuzzles();
        SetSprings();
    }
    void SetBarrels()
    {
        barrelsDropdown.ClearOptions();
        List<string> names = new();
        foreach (var key in ComponentInventory.Instance.BarrelComponents.Keys)
        {
            names.Add(key);
        }
        barrelsDropdown.AddOptions(names);
    }

    void SetMagazines()
    {
        magazineDropdown.ClearOptions();
        List<string> names = new();
        foreach (var key in ComponentInventory.Instance.MagazineComponents.Keys)
        {
            names.Add(key);
        }
        magazineDropdown.AddOptions(names);
    }

    void SetGrips()
    {
        gripDropdown.ClearOptions();
        List<string> names = new();
        foreach (var key in ComponentInventory.Instance.GripComponents.Keys)
        {
            names.Add(key);
        }
        gripDropdown.AddOptions(names);
    }
    
    void SetMuzzles()
    {
        muzzleDropdown.ClearOptions();
        List<string> names = new();
        foreach (var key in ComponentInventory.Instance.MuzzleComponents.Keys)
        {
            names.Add(key);
        }
        muzzleDropdown.AddOptions(names);
    }

    void SetSprings()
    {
        springDropdown.ClearOptions();
        List<string> names = new();
        foreach (var key in ComponentInventory.Instance.SpringComponents.Keys)
        {
            names.Add(key);
        }
        springDropdown.AddOptions(names);
    }

    private void GetCanvasList()
    {
        _canvasList.Clear();
        var canvasObjects = FindObjectsOfType<Canvas>();
        foreach (var canvas in canvasObjects)
        {
            if (!_canvasList.Contains(canvas) && canvas.renderMode != RenderMode.WorldSpace)
            {
                _canvasList.Add(canvas);
            }
        }
        
    }

    private void ChangeActiveCanvas(bool active)
    {
        foreach (var canvas in _canvasList)
        {
            if (canvas.name != "PauseCanvas")
            {
                canvas.gameObject.SetActive(active);
            }
        }
    }

    public void ChangeBarrel(ComponentInitializer componentInitializer)
    {
        string currentBarrel = barrelsDropdown.options[barrelsDropdown.value].text;
        componentInitializer.ChangeBarrel(currentBarrel);
    }
    public void ChangeGrip(ComponentInitializer componentInitializer)
    {
        string currentGrip = gripDropdown.options[gripDropdown.value].text;
        componentInitializer.ChangeGrip(currentGrip);
    }

    public void ChangeMagazine(ComponentInitializer componentInitializer)
    {
        string currentMagazine = magazineDropdown.options[magazineDropdown.value].text;
        componentInitializer.ChangeMagazine(currentMagazine);
    }

    public void ChangeMuzzle(ComponentInitializer componentInitializer)
    {
        string currentMuzzle = muzzleDropdown.options[muzzleDropdown.value].text;
        componentInitializer.ChangeMuzzle(currentMuzzle);
    }

    public void ChangeSpring(ComponentInitializer componentInitializer)
    {
        string currentSpring = springDropdown.options[springDropdown.value].text;
        componentInitializer.ChangeSpring(currentSpring);
    }
}
