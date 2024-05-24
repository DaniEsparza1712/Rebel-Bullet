using System;
using Cinemachine;
using UnityEngine;
using UnityEngine.Events;

public class EventDoor : MonoBehaviour
{
    [Header("Conditions")]
    [SerializeField]
    private float conditionNum;
    private float _conditionCounter;

    [Header("Animator")]
    [SerializeField]
    private Animator animator;

    public UnityEvent conditionEvent;

    private void Start()
    {
        if(animator)
            conditionEvent.AddListener(OpenDoor);
        if (conditionNum == 0)
        {
            conditionEvent.Invoke();
        }
    }

    public void AddCondition()
    {
        _conditionCounter++;
        if(_conditionCounter >= conditionNum)
            conditionEvent.Invoke();
    }

    public void OpenDoor()
    {
        animator.SetBool("Opened", true);
    }

    public void CloseDoor()
    {
        animator.SetBool("Opened", false);
    }

    public void SetAnim(string anim)
    {
        animator.SetTrigger(anim);
    }
}
