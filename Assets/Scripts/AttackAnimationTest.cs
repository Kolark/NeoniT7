using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackAnimationTest : MonoBehaviour
{
    [SerializeField] Animator animator; 
    public void OnAttackFinished() {
        animator.SetBool("isAttacking", false);
    }
}
