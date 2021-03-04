using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndAttackBehaviour : StateMachineBehaviour
{
    [SerializeField] bool canChangeMove;
    [SerializeField] bool canChangeFlip;
    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (canChangeMove)
        {
            BasicCharacter.Instance.canMove = false;
        }
        if (canChangeFlip)
        {
            BasicCharacter.Instance.canFlip = false;
        }
    }
    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (canChangeMove)
        {
            BasicCharacter.Instance.canMove = true;
        }
        if (canChangeFlip)
        {
            BasicCharacter.Instance.canFlip = true;
        }

    }
}
