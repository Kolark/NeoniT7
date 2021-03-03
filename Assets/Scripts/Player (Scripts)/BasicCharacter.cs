using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class BasicCharacter : MonoBehaviour
{
    #region singleton
    private static BasicCharacter instance;
    public static BasicCharacter Instance { get => instance; }
    #endregion
    #region components
    protected CharacterMovement character;
    protected InputController inputController;
    public CharacterMovement Character { get => character; }
    #endregion
    #region AttackInfos
    [Header("AttackPositions")]
    [SerializeField] protected AttackInfo firstAttack;
    [SerializeField] protected AttackInfo secondAttack;
    [SerializeField] protected AttackInfo thirdAttack;
    [SerializeField] protected AttackInfo airAttack;
    [SerializeField] protected AttackInfo crouchAttack;
    [SerializeField] protected AttackInfo counter;
    [Space]
    #endregion
    #region booleans
    [Header("Bools")]
    public bool canReceiveInput;
    public bool canMove = true;
    public bool canFlip = true;
    public bool canReceiveDamage = true;
    protected bool isAlive = true;
    [Space]
    #endregion
    #region Attributes
    [Header("Attributes")]
    [SerializeField] protected int maxLife;
    [SerializeField] protected float cdDefense;
    [SerializeField] protected float cdUltimate;
    [SerializeField] protected float cdThrow;
    public Action onAttack;
    public attackTypes currentAttack;
    #endregion
    protected virtual void Awake()
    {
        canReceiveInput = true;
        if (instance != null)
        {
            Destroy(this.gameObject);
        }
        instance = this;
        character = GetComponent<CharacterMovement>();
    }
    protected virtual void Start()
    {
        inputController = InputController.Instance;
        inputController.Jump += character.Jump;
        inputController.Attack += Attack;
        inputController.DefensiveAbility += Defense;
        inputController.SpecialAbility += Ultimate;
        inputController.Throw += Throwable;
    }
    protected virtual void Update()
    {
        if (!isAlive) return;
        character.UpdateAnimatorValues();
    }
    protected virtual void FixedUpdate()
    {
        if (!isAlive) return;
        character.Crouch();
        if (canFlip)
        {
            character.Flip();
        }
        if (canMove && !character.IsCrouching)
        {
            character.Move();
        }
    }
    #region Attacks
    public virtual void Attack()
    {
        if (canReceiveInput)
        {
            //isAttacking = true;
            canReceiveInput = false;
            character.CanJump = false;
            switch (currentAttack)
            {
                case attackTypes.First:
                    AttackOne();
                    break;
                case attackTypes.Second:
                    AttackTwo();
                    break;
                case attackTypes.Third:
                    AttackThree();
                    break;
                case attackTypes.Airbone:
                    AirAttack();
                    break;
                case attackTypes.Crouch:
                    CrouchAttack();
                    break;
                case attackTypes.Counter:
                    Counter();
                    break;

            }
            onAttack?.Invoke();
        }
    }
    public virtual void AttackOne()
    {
        Collider2D Hit = Physics2D.OverlapCircle(firstAttack.pos.position, firstAttack.radius, firstAttack.layer);
        IEnemyHurtBox enemy = Hit?.GetComponent<IEnemyHurtBox>();
        enemy?.OnReceiveDamage();
    }
    public virtual void AttackTwo()
    {
        Collider2D Hit = Physics2D.OverlapCircle(secondAttack.pos.position, secondAttack.radius, secondAttack.layer);
        IEnemyHurtBox enemy = Hit?.GetComponent<IEnemyHurtBox>();
        enemy?.OnReceiveDamage();
    }
    public virtual void AttackThree()
    {
        Collider2D[] Hit = Physics2D.OverlapCircleAll(thirdAttack.pos.position, thirdAttack.radius, thirdAttack.layer);
        for (int i = 0; i < Hit.Length; i++)
        {
            IEnemyHurtBox enemy = Hit[i]?.GetComponent<IEnemyHurtBox>();
            enemy?.OnReceiveDamage();
        }

    }
    public virtual void AirAttack()
    {
        Collider2D Hit = Physics2D.OverlapCircle(airAttack.pos.position, airAttack.radius, airAttack.layer);
        IEnemyHurtBox enemy = Hit?.GetComponent<IEnemyHurtBox>();
        enemy?.OnReceiveDamage();
    }
    public virtual void CrouchAttack()
    {
        Collider2D Hit = Physics2D.OverlapBox(crouchAttack.pos.position, Vector2.right*crouchAttack.radius + Vector2.up, 0, crouchAttack.layer);
        IEnemyHurtBox enemy = Hit?.GetComponent<IEnemyHurtBox>();
        enemy?.OnReceiveDamage();
    }
    #endregion

    public virtual void Defense()
    {
        if (!isAlive) return;
        character.Anim.SetTrigger("Parry");
        //SetTrigger(Defense)
        //SAmurai-Se vuelve invulnerable al siguiente ataque - Y Cuando recibe daño hace un ataque. - No se cancela
        //NINJA- Se vuelve invulnerable durante toda la animación. Cambia de layer para que pueda atravesarlos y no hacer daño.- DASH- No se cancela
        //YAKUZA- Se vuelve invulnerable durante toda la nimación, al inicio empuja a todos los enemigos, se rompe al momento que lo golpean- No se cancela,tampoco se mueve
    }
    public virtual void Ultimate()
    {
        if (!isAlive) return;
        character.Anim.SetTrigger("Special");
    }
    
    public virtual void Throwable()
    {
        if (!isAlive) return;
        character.Anim.SetTrigger("Throw");
    }
    public virtual void Damage()
    {
        if (canReceiveDamage)
        {
            maxLife--;
            bool isDead = maxLife <= 0;
            if (isDead)
            {
                isAlive = false;
                character.Anim.SetTrigger("Death");
            }
        }
    }
    public virtual void Counter()
    {

    }
    public virtual void StartParry()
    {

    }
    public virtual void EndParry()
    {

    }

    protected virtual void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        switch (currentAttack)
        {
            case attackTypes.First:
                Gizmos.color = Color.red;
                Gizmos.DrawWireSphere(firstAttack.pos.position, firstAttack.radius);
                break;
            case attackTypes.Second:
                Gizmos.color = Color.blue;
                Gizmos.DrawWireSphere(secondAttack.pos.position, secondAttack.radius);
                break;
            case attackTypes.Third:
                Gizmos.color = Color.magenta;
                Gizmos.DrawWireSphere(thirdAttack.pos.position, thirdAttack.radius);
                break;
            case attackTypes.Airbone:
                Gizmos.color = Color.yellow;
                Gizmos.DrawWireSphere(airAttack.pos.position, airAttack.radius);
                break;
            case attackTypes.Crouch:
                Gizmos.color = Color.cyan;
                Gizmos.DrawCube(crouchAttack.pos.position, Vector2.right * crouchAttack.radius + Vector2.up);
                //Gizmos.DrawWireSphere(crouchAttack.pos.position, crouchAttack.radius);
                break;
        }
        
    }
    
}
public enum attackTypes
{
    First,Second,Third,Airbone,Crouch,Counter
}
[Serializable]
public struct AttackInfo
{
    public Transform pos;
    public float radius;
    public LayerMask layer;
}