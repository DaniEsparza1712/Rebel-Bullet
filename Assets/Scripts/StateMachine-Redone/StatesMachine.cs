using UnityEngine;
using UnityEngine.AI;

public class StatesMachine : MonoBehaviour
{
    protected Animator _animator;
    
    protected StateBase _initialState;
    protected StateBase _currentState;
    
    protected void Awake()
    {
        _animator = GetComponent<Animator>();
    }
    
    // Start is called before the first frame update
    protected void Start()
    {
        _currentState.Enter();
    }

    // Update is called once per frame
    protected void Update()
    {
        _currentState.UpdateState();
    }

    private void FixedUpdate()
    {
        _currentState.UpdatePhysics();
    }
    
    protected void ChangeState(StateBase state)
    {
        _currentState.Exit();
        state.Enter();
        _currentState = state;
    }
}
