using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OverheatSystem : MonoBehaviour, ComponentInterface
{
    public enum Stage
    {
        idle,
        shooting,
        waiting,
        recovering
    }
    public Stage stage;
    float timer;

    [Header("Overheat")]
    public float overheatMax;
    private float _overheatRate;
    private float _overheatRecoverySpeed;
    public float overheatCurrent;
    public bool onOverheat;
    private float _recoveryWaitTime;
    public Image overheatImg;

    public void ChangeChildrenStats()
    {
        _overheatRate = 1 / ComponentHolder.Instance.OverheatRate;
        _overheatRecoverySpeed = ComponentHolder.Instance.OverheatRate/2;
    }
    // Start is called before the first frame update
    void Start()
    {
        stage = Stage.idle;
        _overheatRate = 1 / ComponentHolder.Instance.OverheatRate;
        _overheatRecoverySpeed = ComponentHolder.Instance.OverheatRate/2;
        _recoveryWaitTime = 0.5f;
        onOverheat = false;
        timer = 0;
    }

    // Update is called once per frame
    void Update()
    {
        SetImgFillAmount();
        SetImgColor();
        if (stage == Stage.waiting)
        {
            timer += Time.deltaTime;
        }
        if (stage == Stage.shooting && overheatCurrent >= overheatMax)
        {
            onOverheat = true;
            stage = Stage.waiting;
        }

        if (GetShootingStageCondition() && (Input.GetButton("Fire1") || Input.GetAxisRaw("Fire1") == 1))
        {
            stage = Stage.shooting;
        }
        else if (stage == Stage.shooting && (Input.GetButtonUp("Fire1") || Input.GetAxisRaw("Fire1") <= 0))
        {
            stage = Stage.waiting;
        }
        else if (stage == Stage.waiting && timer >= _recoveryWaitTime)
        {
            timer = 0;
            stage = Stage.recovering;
        }
        else if(stage == Stage.recovering && overheatCurrent > 0)
            overheatCurrent = Mathf.Clamp(overheatCurrent - _overheatRecoverySpeed * Time.deltaTime, 0, overheatMax);
        else if(stage == Stage.recovering && overheatCurrent <= 0){
            onOverheat = false;
            stage = Stage.idle;
        }
    }
    public void AddOverheat()
    {
        overheatCurrent = Mathf.Clamp(overheatCurrent + _overheatRate, 0, overheatMax);
    }

    void SetImgFillAmount()
    {
        overheatImg.fillAmount = Mathf.Lerp(0, 1, overheatCurrent / overheatMax);
    }
    void SetImgColor()
    {
        overheatImg.color = Color.Lerp(Color.green, Color.red, overheatCurrent / overheatMax);
    }
    bool GetShootingStageCondition()
    {
        return stage == Stage.idle || !onOverheat && stage == Stage.recovering || !onOverheat && stage == Stage.waiting;
    }
}
