using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransitionBehaviour : StateMachineBehaviour
{
    [Header("TransitionAttributes")]
    [SerializeField] string animation;
    [SerializeField] attackTypes AttackType;
    [SerializeField] string gamerName;
    [Space]
    protected Animator transitionAnimator;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        transitionAnimator = animator;
        //Debug.Log(gamerName + "Enter");
        BasicCharacter.Instance.canReceiveInput = true;
        BasicCharacter.Instance.currentAttack = AttackType;
        BasicCharacter.Instance.onAttack += OnAttack;

    }
    public virtual void OnAttack()
    {
        transitionAnimator.SetTrigger(animation);
    }
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        BasicCharacter.Instance.onAttack -= OnAttack;
        BasicCharacter.Instance.canReceiveInput = false;
        //Debug.Log(gamerName + "Exit");

    }
}
