using System.Collections;
using System.Collections.Generic;
using UnityEngine;

enum EnemyType {Base, Distance, Wall}
public class EnemyController : MonoBehaviour
{
    [Header("Attributes")]
    [SerializeField] int maxHealth;
    [SerializeField] int currentHealth;

    //WIP: Need to add different attack colliders. 
 
    bool isAlive = true;//<-----------------------

    #region Private Variables
    private IStateMachineAI ai; 
    public IStateMachineAI Ai { get => ai;}

    Animator animator;
    bool isDead;
    bool isAttacking;
    #endregion
    private void Awake()
    {
        ai = GetComponent<IStateMachineAI>();
        animator = GetComponentInChildren<Animator>();
        currentHealth = maxHealth;
    }
    void FixedUpdate()
    {
        if (!isAlive) return;
        ai.StateMachine();
    }

    public void ReceiveDamage()
    {
        if (!isAlive) return;
        currentHealth--;
        if (currentHealth < 0)
        {
            isAlive = false;
            animator.SetTrigger("Dead");
        }
    }
  
}
