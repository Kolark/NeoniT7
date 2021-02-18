using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicCharacter : MonoBehaviour
{
    private static BasicCharacter instance;
    public static BasicCharacter Instance { get => instance; }

    CharacterMovement character;
    InputController inputController;
    [SerializeField] LayerMask hurtBoxEnemy;
    [SerializeField] Transform pos;

    public bool canReceiveInput;
    public bool inputReceived;

    private void Awake()
    {
        canReceiveInput = true;
        inputReceived = false;

        if (instance != null)
        {
            Destroy(this.gameObject);
        }
        instance = this;

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
        if (canReceiveInput)
        {
            inputReceived = true;
            canReceiveInput = false;

            Collider2D Hit = Physics2D.OverlapPoint(pos.position, hurtBoxEnemy);
            IEnemyHurtBox enemy = Hit?.GetComponent<IEnemyHurtBox>();
            enemy?.OnReceiveDamage();
        }
        else
        {
            return;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(pos.position, 0.25f);
    }

    public void AnimatorInputManager()
    {
        if (!canReceiveInput) canReceiveInput = true;
        else canReceiveInput = false;
    }
}
