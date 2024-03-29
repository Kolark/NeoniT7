﻿using System.Collections;
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
    SoundModule soundModule;

    #region Private Variables
    private IStateMachineAI ai; 
    public IStateMachineAI Ai { get => ai;}
    EffectsModule effectsModule;
    Animator animator;
    Rigidbody2D rb; 
    RigidbodyConstraints2D rbc; 
    bool isDead;
    bool isAttacking;
    [SerializeField]
    float spawnAnimationSeconds;
    float spawntimer;
    #endregion
    private void Awake()
    {
        ai = GetComponent<IStateMachineAI>();
        animator = GetComponentInChildren<Animator>();
        currentHealth = maxHealth;
        effectsModule = GetComponent<EffectsModule>();
        rb = GetComponent<Rigidbody2D>();
        soundModule = GetComponent<SoundModule>();
        rbc = rb.constraints;
        spawntimer = 0;
    }
    private void Start()
    {
        soundModule.Play((int)EnemySounds.Appear);
    }
    void FixedUpdate()
    {
        if (!isAlive) return;
        ai.StateMachine();
        spawntimer += Time.deltaTime;
    }

    public void ReceiveDamage()
    {
        if (spawntimer >= spawnAnimationSeconds)
        {
            if (!isAlive) return;
            currentHealth--;
            effectsModule.PlayEffect((int)effectsOniA.hit);
            if (currentHealth < 0)
            {
                Death();
            }
        }
    }

    public void Death()
    {

        isAlive = false;
        animator.SetTrigger("Dead");
        gameObject.layer = 13;
        Invoke("DestroyEnemy", 1.5f); // Debug only 
        soundModule.Play((int)EnemySounds.Disappear);
        chamber.OnEnemyDead(this);
    }


    public void InstaDeath()
    {
        isAlive = false;
        animator.SetTrigger("Dead");
        gameObject.layer = 13;
        Destroy(this.gameObject); // Debug only 
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


    public enum effectsOniA
    {
        hit, portal
    }

}
public enum EnemySounds
{
    Attack,Appear,Disappear
}