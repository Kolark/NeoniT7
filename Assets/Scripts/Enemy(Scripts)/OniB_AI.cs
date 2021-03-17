using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;


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
    [SerializeField] GameObject attackPosition;
    [SerializeField] LayerMask playerLayer;

    float spawntimer = 0;

    float timer = 0;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponentInChildren<Animator>();
    }

    IEnumerator Start() //Refactor into GetTarget()
    {
        yield return new WaitUntil(() => BasicCharacter.Instance != null);
        //target = BasicCharacter.Instance.transform;
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
        animator.SetBool("isRunning", false);
        GetTarget();
        //If in distance start chase
        // if (TargetInDistance() && followEnabled)
        // {
        //     InvokeRepeating("UpdateMovementPath", 0f, pathUpdateSeconds);
        //     currentState = OniBStates.Chase;
        // }
        //else enemyMovement.seeker.enabled = false;
    }

    public void Attack()
    {
        if (canAttack)
        {
            animator.SetTrigger("Attack");
            //Collider2D Hit = Physics2D.OverlapCircle(attackInfo.pos.position, attackInfo.radius, attackInfo.layer);
            // if (Hit != null)
            // {
            //     PlayerDamageHandler player = Hit.GetComponent<PlayerDamageHandler>();
            //     if (player != null)
            //     {
            //         player.OnReceiveDamage();
            //     }
            // }
            canAttack = false;
        }
    }

    public void OnAttackEnd()
    {
        Debug.Log("AttackEnd-END");
        canAttack = true;
        if (target == null) currentState = OniBStates.Idle;
        //else currentState = OniBStates.Chase;
    }

    void GetTarget() {
        RaycastHit2D playerDetection = Physics2D.Raycast(attackPosition.transform.position, GetDirection(), detectionRange, playerLayer);
        playerDetected = playerDetection.collider;

        if (playerDetected) target = BasicCharacter.Instance.transform;
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
        Gizmos.DrawLine(attackPosition.transform.position, (Vector2)attackPosition.transform.position + GetDirection() * detectionRange);
    }
}
