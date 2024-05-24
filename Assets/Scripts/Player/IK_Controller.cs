using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IK_Controller : MonoBehaviour
{
    protected Animator animator;
    public bool lookAt;
    public bool pointAt;
    float armLerp;
    public Transform lookAtTr;
    public Transform rightHandTr;
    public Transform leftHandTr;

    public Transform rightElbowTr;
    public Transform leftElbowTr;

    // Start is called before the first frame update
    void Start()
    {
        armLerp = 0;
        animator = GetComponent<Animator>();
    }
    private void Update() {
        if(armLerp < 1 && pointAt)
            armLerp += 5f * Time.deltaTime;
        else if(armLerp > 0 && !pointAt)
            armLerp -= 5f * Time.deltaTime;
    }

    private void OnAnimatorIK() {
        if(lookAt){
            animator.SetLookAtWeight(1);
            animator.SetLookAtPosition(lookAtTr.position);
        }
        else{
            animator.SetLookAtWeight(0);
        }
        animator.SetIKPositionWeight(AvatarIKGoal.RightHand, armLerp);
        animator.SetIKRotationWeight(AvatarIKGoal.RightHand, armLerp);
        animator.SetIKPosition(AvatarIKGoal.RightHand, rightHandTr.position);
        animator.SetIKRotation(AvatarIKGoal.RightHand, rightHandTr.rotation);

        animator.SetIKPositionWeight(AvatarIKGoal.LeftHand, armLerp);
        animator.SetIKRotationWeight(AvatarIKGoal.LeftHand, armLerp);
        animator.SetIKPosition(AvatarIKGoal.LeftHand, leftHandTr.position);
        animator.SetIKRotation(AvatarIKGoal.LeftHand, leftHandTr.rotation);

        animator.SetIKHintPositionWeight(AvatarIKHint.RightElbow, armLerp);
        animator.SetIKHintPosition(AvatarIKHint.RightElbow, rightElbowTr.position);
        animator.SetIKHintPositionWeight(AvatarIKHint.LeftElbow, armLerp);
        animator.SetIKHintPosition(AvatarIKHint.LeftElbow, leftElbowTr.position);

        
    }
    void GetRightFootPos()
    {

    }
}
