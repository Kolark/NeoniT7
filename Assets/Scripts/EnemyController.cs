using System.Collections;
using System.Collections.Generic;
using UnityEngine;

enum EnemyType {Base, Distance, Wall}
public class EnemyController : MonoBehaviour
{
    [Header("Attributes")]
    [SerializeField] int maxHealth;
    [SerializeField] int currentHealth;
    [SerializeField] int damage; //Private, might need to add a property if an external difficulty script needs to change it.
    [SerializeField] EnemyState startingState;
    [SerializeField] float attackDistance;

    [Header("AttackColliders")]
    [SerializeField] LayerMask playerMask;
    [SerializeField] LayerMask playerHurtBox;
    //WIP: Need to add different attack colliders. 
    [SerializeField] AttackInfo attackInfo;
    #region Private Variables
    private EnemyAI ai; 
    Animator animator;
    bool isDead;
    bool isAttacking;

    public float AttackDistance { get => attackDistance; }
    public EnemyAI Ai { get => ai;}
    #endregion
    private void Awake()
    {
        ai = GetComponent<EnemyAI>();
        ai.State = startingState;
        animator = GetComponentInChildren<Animator>();
        currentHealth = maxHealth;
    }
    void FixedUpdate()
    {
        ai.PathfindingLogic();
    }

    // public void OnReceiveDamage() {
    //     Destroy(gameObject);
    // }

    public void Attack() 
    {
        Collider2D Hit = Physics2D.OverlapCircle(attackInfo.pos.position, attackInfo.radius, attackInfo.layer);
        Debug.Log("Attack step 1");
        if (Hit != null)
        {
            Debug.Log("Attack step 2");

            PlayerDamageHandler player = Hit.GetComponent<PlayerDamageHandler>();
            if (player != null)
            {
                player.OnReceiveDamage();
                Debug.Log("Attack step 3");
            }
        }
        
        
        
        //IEnemyHurtBox player = Hit?.GetComponent<IEnemyHurtBox>();
        //player?.OnReceiveDamage();        
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(attackInfo.pos.position, attackInfo.radius);
    }
}
