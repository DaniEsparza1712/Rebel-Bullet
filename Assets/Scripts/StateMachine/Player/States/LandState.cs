using UnityEngine;

public class LandState : State
{
    private PlayerMachine _sm;

    public LandState(PlayerMachine pm) : base(pm)
    {
        _sm = pm;
    }
    
    public override void Enter()
    {
        _sm.rb.isKinematic = true;
        _sm.mainCollider.enabled = false;
        _sm.character.enabled = true;
        
        _sm.ik_Controller.pointAt = false;
        _sm.rig.weight = 0;

        _sm.lockOnIcon.SetActive(false);
        _sm.weaponAppear.Disappear();
        _sm.animator.SetTrigger("Land");
        _sm.animator.SetTrigger("FE_Neutral");
        _sm.character.Move(Vector3.zero);
    }
    public override void UpdateLogic(){
        if (_sm.changeTo == "idle")
        {
            _sm.ChangeState(_sm.baseState);
            if(_sm.currentState != this)
                _sm.changeTo = "";
        }
    }
    public override void FixedUpdateLogic(){
        
    }
}
