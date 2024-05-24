using UnityEngine;

public class BaseState : State
{
    public PlayerMachine sm;
    Vector3 direction;
    float viewAngle;
    Vector3 viewDir;
    float hAxis;
    float vAxis;
    float rotationMod = 100;
    public BaseState(PlayerMachine pm): base(pm){
        sm = pm;
    }
    public override void Enter()
    {
        sm.rb.isKinematic = true;
        sm.mainCollider.enabled = false;
        sm.character.enabled = true;
        
        sm.ik_Controller.pointAt = false;
        sm.rig.weight = 0;

        sm.lockOnIcon.SetActive(false);
        sm.weaponAppear.Disappear();
        sm.animator.SetTrigger("Base");
        sm.animator.SetTrigger("FE_Neutral");
        sm.character.Move(Vector3.zero);
        sm.speed = sm.baseSpeed;
        
        if (sm.cameraSwitcher.lockOnCamera.enabled)
        {
            sm.StartCoroutine(sm.SnapCamera());
            sm.cameraSwitcher.SwitchCamera(CameraSwitcher.Cameras.main);
        }
        else
            sm.cameraSwitcher.SwitchCamera(CameraSwitcher.Cameras.main);
    }
    public override void UpdateLogic(){
        hAxis = Input.GetAxis("Horizontal");
        vAxis = Input.GetAxis("Vertical");
        direction = new Vector3(hAxis, 0, vAxis);
        sm.animator.SetFloat("Walk", direction.magnitude);

        viewAngle = -Camera.main.transform.eulerAngles.y;

        viewDir = MathUtilities.TurnVector(direction, viewAngle);
        
        if(Input.GetButtonDown("Jump")){
            sm.ChangeState(sm.jumpState);
        }
        else if(!sm.OnGround()){
            sm.ChangeState(sm.fallState);
        }
        else if(Input.GetAxisRaw("Lock On") == 1 || Input.GetButtonDown("Lock On")){
            sm.ChangeState(sm.lockOnBase);
        }
        else if(Input.GetButton("Fire1") || Input.GetAxisRaw("Fire1") == 1){
            sm.ChangeState(sm.lockOnBase);
        }
        else if (Input.GetButtonDown("AttackL"))
        {
            sm.ChangeState(sm.AttackState);
        }
        else if (Input.GetButtonDown("AttackH"))
        {
            sm.ChangeState(sm.AttackHState);
        }
        else if (Input.GetButtonDown("Dash"))
        {
            sm.ChangeState(sm.DashState);
        }
    }
    public override void FixedUpdateLogic(){
        sm.character.Move(viewDir * sm.speed + Physics.gravity/50.0f);
        if(viewDir.magnitude > 0 + Mathf.Epsilon)
            sm.transform.rotation = Quaternion.RotateTowards(sm.transform.rotation, Quaternion.LookRotation(viewDir), sm.speed * rotationMod);
    }
}
