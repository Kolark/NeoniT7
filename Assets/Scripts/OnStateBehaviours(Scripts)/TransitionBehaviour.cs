using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransitionBehaviour : StateMachineBehaviour
{
    [Header("TransitionAttributes")]
    [SerializeField] string animation;
    [SerializeField] string gamerName;
    [Space]
    protected Animator transitionAnimator;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        //Debug.Log("Enter" + gamerName);
        transitionAnimator = animator;
        BasicCharacter.Instance.canReceiveInput = true;
        BasicCharacter.Instance.onAttack += OnAttack;

    }
    public virtual void OnAttack()
    {
        transitionAnimator.SetTrigger(animation);
    }
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        //Debug.Log("Exit" + gamerName);
        BasicCharacter.Instance.onAttack -= OnAttack;
        BasicCharacter.Instance.canReceiveInput = false;
        
    }
}
