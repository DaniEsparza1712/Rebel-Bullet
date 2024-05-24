using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateMachine : MonoBehaviour
{
    [HideInInspector]
    public State currentState;
    public Manager Manager;
    // Start is called before the first frame update
    void Start()
    {
        Manager = GameObject.Find("Manager").GetComponent<Manager>();
        currentState.Enter();
    }

    // Update is called once per frame
    void Update()
    {
        currentState.UpdateLogic();
    }

    void FixedUpdate() {
        if (Time.timeScale > 0 && Manager.hasControl)
            currentState.FixedUpdateLogic();
    }

    public void ChangeState(State newState){
        if (Time.timeScale > 0 && Manager.hasControl)
        {
            currentState = newState;
            currentState.Enter();
        }
    }
}
