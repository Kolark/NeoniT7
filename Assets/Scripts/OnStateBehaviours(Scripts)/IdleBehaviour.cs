using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleBehaviour : TransitionBehaviour
{
    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnStateEnter(animator, stateInfo, layerIndex);
        BasicCharacter.Instance.Character.CanJump = true;
        //Debug.Log("Idle");
    }
    public override void OnAttack()
    {
        base.OnAttack();

    }

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnStateExit(animator, stateInfo, layerIndex);
    }
}
