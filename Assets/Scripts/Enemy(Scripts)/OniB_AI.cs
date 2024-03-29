﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;
using DG.Tweening;


public class OniB_AI : MonoBehaviour,IStateMachineAI
{
    public enum OniBStates
    {
        Spawn,Idle, Attack
    }

    Animator animator;
    Rigidbody2D rb;
    [SerializeField] Transform target;
    OniBStates currentState = OniBStates.Spawn;

    bool canAttack = true;
    bool playerDetected;
    public float detectionRange = 50f;
    [SerializeField] float attackDistance;
    [SerializeField] float attackRate = 2f;
    [SerializeField] float spawnAnimationSeconds;
    [SerializeField] GameObject projectile;
    [SerializeField] Transform attackPosition;
    [SerializeField] LayerMask playerLayer;
    [SerializeField] float shootForce;
    SoundModule soundModule;
    float spawntimer = 0;

    float timer = 0;
    float nextAttackTime = 0f;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponentInChildren<Animator>();
        soundModule = GetComponent<SoundModule>();
    }

    IEnumerator Start() //Refactor into GetTarget()
    {
        yield return new WaitUntil(() => BasicCharacter.Instance != null);
    }

    public void StateMachine()
    {
        switch (currentState)
        {
            case OniBStates.Spawn:
                spawntimer += Time.deltaTime;
                if(spawntimer > spawnAnimationSeconds)
                {
                    currentState = OniBStates.Idle;
                }
                break;

            case OniBStates.Idle:
                Idle();
                break;
            case OniBStates.Attack:
                Attack();
                break;
        }
    }

    public void Idle()
    {
        GetTarget();
        if (target != null) {
            currentState = OniBStates.Attack;
        } else currentState = OniBStates.Idle;
    }

    public void Attack()
    {
        if (canAttack)
        {
            animator.SetTrigger("Attack");
            canAttack = false;
            DOVirtual.DelayedCall(0.8f, null, true).OnComplete(() =>
            {
                soundModule.Play((int)EnemySounds.Attack);
                GameObject clone = Instantiate(projectile, attackPosition.position, attackPosition.rotation);
                clone.transform.localScale = new Vector3(GetDirection().x, clone.transform.localScale.y, clone.transform.localScale.z);
                clone.GetComponent<Rigidbody2D>().AddForce(GetDirection() * shootForce, ForceMode2D.Impulse);
                nextAttackTime = Time.time + 1f / attackRate;
            });
                  
        }
        GetTarget();
    }

    public void OnAttackEnd()
    {
        Debug.Log("AttackEnd-END");
        canAttack = true;
        if (target == null) currentState = OniBStates.Idle;
        //else currentState = OniBStates.Chase;
    }

    void GetTarget() {
        RaycastHit2D playerDetection = Physics2D.Raycast(attackPosition.position, GetDirection(), detectionRange, playerLayer);
        playerDetected = playerDetection.collider;

        if (playerDetected) target = BasicCharacter.Instance.transform;
        else if (!playerDetected) target = null;
        Flip();
        //else if (!playerDetected) target = null;
    }

    void Flip() {
        float distanceToPlayer = (transform.position.x - BasicCharacter.Instance.transform.position.x);
        if (distanceToPlayer > 0) transform.localScale = new Vector3(-1,1,1);
        else if (distanceToPlayer < 0) transform.localScale = new Vector3(1,1,1);
    }

    Vector2 GetDirection() {
        Vector2 direction = Vector2.zero;
        if (transform.localScale.x > 0) direction = Vector2.right;
        else if (transform.localScale.x < 0) direction = Vector2.left;
        return direction;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(attackPosition.position, (Vector2)attackPosition.position + GetDirection() * detectionRange);
    }
}
