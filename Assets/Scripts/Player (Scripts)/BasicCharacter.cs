using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class BasicCharacter : MonoBehaviour
{
    private static BasicCharacter instance;
    public static BasicCharacter Instance { get => instance; }

    CharacterMovement character;
    InputController inputController;
    [SerializeField] LayerMask hurtBoxEnemy;
    [SerializeField] Transform pos;

    public bool canReceiveInput;
    //public bool inputReceived;

    public Action onAttack;

    private void Awake()
    {
        canReceiveInput = true;
        //inputReceived = false;

        if (instance != null)
        {
            Destroy(this.gameObject);
        }
        instance = this;

        character = GetComponent<CharacterMovement>();
        

    }
    private void Start()
    {
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
        if (canReceiveInput)
        {
            canReceiveInput = false;
            Collider2D Hit = Physics2D.OverlapPoint(pos.position, hurtBoxEnemy);
            IEnemyHurtBox enemy = Hit?.GetComponent<IEnemyHurtBox>();
            enemy?.OnReceiveDamage();
            onAttack?.Invoke();
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(pos.position, 0.25f);
    }
}
