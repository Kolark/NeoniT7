using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
/// <summary>
/// This script is responsable for the basic character movement
/// It has a referece to the character animator controller and 5 rays that
/// check if the character is grounded.
/// The movement is done by a character controller
/// </summary>

public class CharacterMovement : MonoBehaviour
{
    #region PrivateVariables
    private Animator anim;
    private CharacterController controller;
    private SoundModule soundModule;
    private Vector3 slopeNormal;
    [SerializeField]private bool grounded;
    public bool facingRight = true;
    private bool isCrouching;
    [SerializeField]private bool canJump = true;
    private float verticalVelocity;
    private Rigidbody2D rb;
    [SerializeField] float MaxYVelocity;

    [SerializeField] float jumpDownDelay;
    EffectsModule effectsModule;

    Vector2 velocity;
    #endregion
    #region SerializedVariables
    [SerializeField] private float groundCheckRadius = 0.2f;
    [SerializeField] private LayerMask groundlayer;
    [SerializeField] private Transform groundCheck;
    //[SerializeField] private float acceleration = 5f;
    [SerializeField] private float maxSpeed = 5f;
    [SerializeField] private float terminalVelocity = 5f;
    [SerializeField] private float jumpForce = 8f;
    [SerializeField] private float fallMultiplier = 10f;

    public bool IsCrouching { get => isCrouching;}
    public bool CanJump { get => canJump; set => canJump = value; }
    public Animator Anim { get => anim;}
    public bool Grounded { get => grounded;}
    public Rigidbody2D Rb { get => rb;}

    private bool isJumping;
    #endregion
    private void Awake()
    {
        soundModule = GetComponent<SoundModule>();
        rb = GetComponent<Rigidbody2D>();
        controller = GetComponent<CharacterController>();
        anim = transform.GetChild(0).GetComponent<Animator>();
        effectsModule = GetComponent<EffectsModule>();
    }
 
    public void Move()
    {
        //BetterJump();
        Vector2 inputVector = GetInput();

        if (!isCrouching)
        {
            Vector2 direcction = new Vector2(inputVector.x, 0);
            transform.Translate(direcction * Time.deltaTime * maxSpeed);
        }
    }

    public void UpdateAnimatorValues()
    {
        Vector2 inputVector = GetInput();
        verticalVelocity = rb.velocity.y;
        anim?.SetFloat("Speed", Mathf.Abs(inputVector.x));
        grounded = GetGrounded();
        anim?.SetBool("IsGrounded", grounded);
        anim?.SetFloat("VerticalVelocity", verticalVelocity);
    }

    public void Crouch()
    {
        Vector2 inputVector = GetInput();
        isCrouching = inputVector.y < -0.9f;
        anim.SetBool("IsCrouching", isCrouching);
    }

    public void Jump()
    {
        if (grounded && canJump && !isCrouching)
        {
            //isJumping = true;
            rb.velocity = new Vector2(rb.velocity.x, 0f);
            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
            anim?.SetTrigger("Jump");
            soundModule.Play((int)CharacterSounds.Jump);
            DOVirtual.DelayedCall(jumpDownDelay, () => {
                if(rb.velocity.y > 0)
                {
                    Vector2 vel = rb.velocity;
                    vel.y = 0;
                    rb.velocity = vel;
                }

            }, true);
            effectsModule?.PlayEffect((int)effectsSamurai.jumpParticle);
        }
    }

    public void BetterJump()
    {
        if (rb.velocity.y < 0 && !grounded)
        {
            rb.velocity += Vector2.up * Physics2D.gravity.y * (fallMultiplier - 1) * Time.deltaTime;
        }
    }

    private Vector2 GetInput()
    {
        return InputController.Instance.Move.normalized;
    }
    public bool GetGrounded()
    {
        //Collider2D hit = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundlayer);
        Collider2D hit = Physics2D.OverlapBox(groundCheck.position, Vector2.right * groundCheckRadius + Vector2.up * 0.35f,0, groundlayer);
        return hit != null;
        
    }
    public void Flip()
    {
        Vector2 inputVector = GetInput();
        if (inputVector.x > 0 && !facingRight)
        {
            facingRight = !facingRight;
            Vector3 theScale = transform.localScale;
            theScale.x *= -1;
            transform.localScale = theScale;
        }
        else if (inputVector.x < 0 && facingRight)
        {
            facingRight = !facingRight;
            Vector3 theScale = transform.localScale;
            theScale.x *= -1;
            transform.localScale = theScale;
        }
    }
    
    private void OnDrawGizmos()
    {
        //Gizmos.color = Color.red;
        //Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireCube(groundCheck.position, Vector2.right * groundCheckRadius + Vector2.up * 0.35f);
    }

    public enum effectsSamurai
    {
        startParry, endParry, jumpParticle, UltReady, PlayerHitA, PlayerHitC, EnergyCharging
    }

}
