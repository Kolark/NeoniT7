using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndParry : StateMachineBehaviour
{

    public override void OnStateEnter(Animator animator, AnimatorStateInfo animatorStateInfo, int layerIndex)
    {
        BasicCharacter.Instance.StartParry();
    }
    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        BasicCharacter.Instance.EndParry();
    }
}
