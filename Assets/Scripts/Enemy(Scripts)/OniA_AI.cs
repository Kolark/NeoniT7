using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;
using DG.Tweening;

[RequireComponent(typeof(EnemyMovement))]
public class OniA_AI : MonoBehaviour,IStateMachineAI
{
    public enum OniAStates
    {
        Spawn,Idle, Chase, Attack
    }
    //[walk,jump],[muerte],[spawn],[idle],[ataque]


    Animator animator;
    Rigidbody2D rb;
    EnemyMovement enemyMovement;
    [SerializeField] Transform target;
    OniAStates currentState = OniAStates.Spawn;

    bool canAttack = true;
    public bool followEnabled = true;
    public float pathUpdateSeconds = 0.5f;
    public float activateDistance = 50f;
    [SerializeField] float attackDistance;
    [SerializeField] float attackRate = 2f;
    [SerializeField] float spawnAnimationSeconds;
    [SerializeField] AttackInfo attackInfo;
    float spawntimer = 0;

    float timer = 0;

    private void Awake()
    {
        enemyMovement = GetComponent<EnemyMovement>();
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponentInChildren<Animator>();
    }

    IEnumerator Start()
    {
        yield return new WaitUntil(() => BasicCharacter.Instance != null);
        target = BasicCharacter.Instance.transform;
    }

    public void StateMachine()
    {
        switch (currentState)
        {
            case OniAStates.Spawn:
                spawntimer += Time.deltaTime;
                if(spawntimer > spawnAnimationSeconds)
                {
                    currentState = OniAStates.Idle;
                }
                break;

            case OniAStates.Idle:
                Idle();
                float distanceToTarget = 0;
                if (target != null) distanceToTarget = Vector2.Distance(transform.position, target.position);

                timer += Time.fixedDeltaTime;

                if (distanceToTarget < attackDistance)//within distance
                {
                    if (timer >= attackRate)
                    {
                        rb.velocity = Vector2.zero;
                        currentState = OniAStates.Attack;
                        timer = 0;
                    }
                }
                break;

            case OniAStates.Chase:
                enemyMovement.Chase();
                //if (!enemyMovement.IsGrounded) return;
                distanceToTarget = 0;
                if (target != null) distanceToTarget = Vector2.Distance(transform.position, target.position);

                timer += Time.fixedDeltaTime;

                if (distanceToTarget < attackDistance)//within distance
                {
                    if (timer >= attackRate)
                    {
                        rb.velocity = Vector2.zero;
                        
                        currentState = OniAStates.Attack;
                        timer = 0;
                    }
                }
                break;
            case OniAStates.Attack:
                Attack();
                break;
        }
    }

    public void Idle()
    {
        animator.SetBool("isRunning", false);
        //If in distance start chase
        if (TargetInDistance() && followEnabled)
        {
            InvokeRepeating("UpdateMovementPath", 0f, pathUpdateSeconds);
            currentState = OniAStates.Chase;
        }
        else enemyMovement.seeker.enabled = false;
    }

    public void Attack()
    {
        if (canAttack)
        {
            animator.SetTrigger("Attack");
            canAttack = false;
            DOVirtual.DelayedCall(0.5f, null, true).OnComplete(() =>
            {
                Collider2D Hit = Physics2D.OverlapCircle(attackInfo.pos.position, attackInfo.radius, attackInfo.layer);
                if (Hit != null)
                {
                    PlayerDamageHandler player = Hit.GetComponent<PlayerDamageHandler>();
                    if (player != null)
                    {
                        player.OnReceiveDamage();
                    }
                }
            });
            
        }
    }

    public void OnAttackEnd()
    {
        Debug.Log("AttackEnd-END");
        canAttack = true;
        if (target == null) currentState = OniAStates.Idle;
        else currentState = OniAStates.Chase;
    }



    public void UpdateMovementPath()
    {
        enemyMovement.UpdatePath();
    }

    /// <summary>
    /// Checks if targe is in range
    /// </summary>
    /// <returns></returns>
    protected bool TargetInDistance()
    {
        if (target != null)
        {
            return Vector2.Distance(transform.position, target.transform.position) < activateDistance;
        }
        else return false;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(attackInfo.pos.position, attackInfo.radius);
        Gizmos.color = Color.black;
        Gizmos.DrawWireSphere(transform.position, attackDistance);
    }
}
