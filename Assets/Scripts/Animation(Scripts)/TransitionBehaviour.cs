using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransitionBehaviour : StateMachineBehaviour
{
    [SerializeField] string animation;
    [SerializeField] string gamerName;
    Animator transitionAnimator;
    [SerializeField] bool canChangeJumpBool = false;
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        //Debug.Log("Enter" + gamerName);
        transitionAnimator = animator;
        BasicCharacter.Instance.canReceiveInput = true;
        BasicCharacter.Instance.onAttack += OnAttack;
        if (canChangeJumpBool)
        {
            BasicCharacter.Instance.Character.CanJump = true;
        }
    }
    public void OnAttack()
    {
        transitionAnimator.SetTrigger(animation);
    }
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        //Debug.Log("Exit" + gamerName);
        BasicCharacter.Instance.onAttack -= OnAttack;
        BasicCharacter.Instance.canReceiveInput = false;
        BasicCharacter.Instance.isAttacking = false;
    }
}
