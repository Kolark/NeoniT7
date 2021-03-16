using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public enum EnemyState { Idle, Chase, Attack, Dead, BackToStart }

public class EnemyAI : MonoBehaviour
{
    //#region Components
    //Animator animator;
    //EnemyController enemy;
    
    //#endregion
    //#region physicsAttributes
    
    //#endregion

    //#region CustomBehaviourAttributes
    //[Header("Custom Behaviour")]
    
    
    
    //public bool attackEnabled = true;
    //[SerializeField] float attackRate = 2f;
    
    //public EnemyState State { get => state; set => state = value; }


    
    //private bool canAttack = true;
    
    
    //EnemyState state;
    //float timer = 0;
    //float nextAttackTime;
    //#endregion

    //private void Awake()
    //{
    //    animator = GetComponentInChildren<Animator>();
    //    enemy = GetComponent<EnemyController>();
        
    //}

    ///// <summary>
    ///// waits until character exists as a singleton and sets it as a Target
    ///// </summary>
    ///// <returns></returns>
    //IEnumerator Start()
    //{
    //    yield return new WaitUntil(() => BasicCharacter.Instance != null);
    //    target = BasicCharacter.Instance.transform;
    //}


    ///// <summary>
    ///// This method will be called on an update. It can be described as a state Machine.
    ///// </summary>
    //public virtual void PathfindingLogic()
    //{
    //    switch (state)
    //    {
    //        #region IDLE
    //        case EnemyState.Idle:
    //            Idle();
    //            break;
    //        #endregion
    //        #region CHASE
    //        case EnemyState.Chase:
    //            if (path == null) return;
    //            if (currentWaypoint >= path.vectorPath.Count) { return; }
    //            Chase();
    //            break;
    //        #endregion
    //        #region ATTACK
    //        case EnemyState.Attack:
    //            Attack();
    //            break;
    //        #endregion
    //        #region DEAD
    //        case EnemyState.Dead:
    //            //TO-DO: Death logic goes here.
    //            break;
    //        #endregion
    //        #region BACKTOSTART
    //        case EnemyState.BackToStart:
    //            //TO-DO: For patrolling or losing player focus, going back to the start logic goes here.
    //            break;
    //            #endregion
    //    }
    //}




    //protected virtual void Idle()
    //{
    //    animator.SetBool("isRunning", false);
    //    //If in distance start chase
    //    if (TargetInDistance() && followEnabled)
    //    {
    //        InvokeRepeating("UpdateMovementPath", 0f, pathUpdateSeconds);
    //        state = EnemyState.Chase;
    //    }
    //    else seeker.enabled = false;
    //}





    ///// <summary>
    ///// Checks if targe is in range
    ///// </summary>
    ///// <returns></returns>
    //protected bool TargetInDistance()
    //{
    //    if (target != null)
    //    {
    //        return Vector2.Distance(transform.position, target.transform.position) < activateDistance;
    //    }
    //    else return false;
    //}

    //protected virtual void Attack()
    //{
    //    if (canAttack)
    //    {
    //        animator.SetTrigger("Attack");
    //        enemy.Attack();
    //        canAttack = false;
    //    }
    //}
    //protected virtual void Jump()
    //{

    //}
    //public virtual void onAttackEnd()
    //{
    //    canAttack = true;
    //    if (target == null) state = EnemyState.Idle;
    //    else state = EnemyState.Chase;
    //}




   



    //private void OnDrawGizmos()
    //{
    //    Gizmos.color = Color.red;
    //    Gizmos.DrawLine(groundDetection.position, (Vector2)groundDetection.position + Vector2.down * 6);
    //    Gizmos.color = Color.cyan;
    //    Gizmos.DrawLine(groundDetection.position, (Vector2)groundDetection.position + Vector2.right * 1f);
    //}

}
