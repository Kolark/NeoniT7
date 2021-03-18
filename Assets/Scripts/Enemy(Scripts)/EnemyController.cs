using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EnemyType {Base, Distance, Wall}
public class EnemyController : MonoBehaviour
{
    [Header("Attributes")]
    [SerializeField] int maxHealth;
    [SerializeField] int currentHealth;

    //WIP: Need to add different attack colliders. 
 
    bool isAlive = true;//<-----------------------
    Chamber chamber;


    #region Private Variables
    private IStateMachineAI ai; 
    public IStateMachineAI Ai { get => ai;}

    Animator animator;
    Rigidbody2D rb; 
    RigidbodyConstraints2D rbc; 
    bool isDead;
    bool isAttacking;
    #endregion
    private void Awake()
    {
        ai = GetComponent<IStateMachineAI>();
        animator = GetComponentInChildren<Animator>();
        currentHealth = maxHealth;
        rb = GetComponent<Rigidbody2D>();
        rbc = rb.constraints;
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
            chamber.OnEnemyDead(this);
            Invoke("DestroyEnemy", 1.5f); // Debug only 
        }
    }

    private void OnCollisionStay2D(Collision2D other) {
        if (other.gameObject.CompareTag("Player")) {
            rb.constraints = RigidbodyConstraints2D.FreezeAll;
        }
    }

    private void OnCollisionExit2D(Collision2D other) {
        if (other.gameObject.CompareTag("Player")) {
            //rb.constraints = RigidbodyConstraints2D.None;
            rb.constraints = rbc;
        }
    }
    
    public void AssignChamber(Chamber chamber){
        this.chamber = chamber;
    }

    //Debug purpose only
    void DestroyEnemy() {
        Destroy(gameObject);
    } 
  
}
