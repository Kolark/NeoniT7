using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConstraintsBehaviour : StateMachineBehaviour
{
    [SerializeField] bool canChangeMove;
    [SerializeField] bool canChangeFlip;
    [SerializeField] bool canChangeJump;
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
        if (canChangeJump)
        {
            BasicCharacter.Instance.Character.CanJump = false;
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
        if (canChangeJump)
        {
            BasicCharacter.Instance.Character.CanJump = true;
        }
    }
}
