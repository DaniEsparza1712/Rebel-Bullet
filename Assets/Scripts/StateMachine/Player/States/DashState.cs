using UniGLTF.MeshUtility;
using UnityEngine;

public class DashState : State
{
    private PlayerMachine _sm;
    private Vector3 _viewDir;
    private float _timer;
    private LifeSystem _life;

    public DashState(PlayerMachine pm) : base(pm)
    {
        _sm = pm;
        _life = pm.gameObject.GetComponent<LifeSystem>();
    }

    public override void Enter()
    {
        _sm.trailObject.SetActive(true);
        _life.SetInvincible(true);
        
        var hAXis = Input.GetAxisRaw("Horizontal");
        var vAxis = Input.GetAxisRaw("Vertical");
        
        var direction = new Vector3(hAXis, 0, vAxis);
        if(direction.magnitude == 0)
            direction = Vector3.forward;
        
        var viewAngle = -Camera.main.transform.eulerAngles.y;
        _viewDir = MathUtilities.TurnVector(direction, viewAngle);
        _timer = 0;

        _sm.transform.rotation = Quaternion.LookRotation(_viewDir);

        _sm.animator.speed = 2;
        _sm.animator.SetFloat("Walk", 1);
    }

    public override void UpdateLogic()
    {
        _timer += Time.deltaTime * Time.timeScale;
        if (_timer >= _sm.dashTime)
        {
            _life.SetInvincible(false);
            _sm.animator.speed = 1;
            _sm.trailObject.SetActive(false);
            if (_sm.OnGround())
            {
                _sm.ChangeState(_sm.baseState);
                _sm.animator.ResetTrigger("Base");
            }
            else if(!_sm.OnGround())
                _sm.ChangeState(_sm.fallState);
        }
    }

    public override void FixedUpdateLogic()
    {
        _sm.character.Move(_viewDir * _sm.dashSpeed);
    }
}
