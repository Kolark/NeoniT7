using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicCharacter : MonoBehaviour
{
    CharacterMovement character;
    InputController inputController;
    [SerializeField] LayerMask hurtBoxEnemy;
    [SerializeField] Transform pos;
    private void Awake()
    {
        character = GetComponent<CharacterMovement>();
        inputController = InputController.Instance;
        inputController.Jump += character.Jump;
        inputController.Attack += Attack;
    }

    private void FixedUpdate()
    {
        character.Move();
        character.Crouch();
    }

    public void Attack()
    {
        Debug.Log("Attack");
        Collider2D Hit = Physics2D.OverlapPoint(pos.position, hurtBoxEnemy);
        IEnemyHurtBox enemy = Hit?.GetComponent<IEnemyHurtBox>();
        enemy?.OnReceiveDamage();
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(pos.position, 0.25f);
    }
}
