using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    private Vector3 slopeNormal;
    private bool grounded;
    private bool facingRight = true;
    private bool isCrouching;
    private bool canJump = true;
    private float verticalVelocity;
    private Rigidbody2D rb;
    
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
    #endregion
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        controller = GetComponent<CharacterController>();
        anim = transform.GetChild(0).GetComponent<Animator>();
    }

    public void Move()
    {
        BetterJump();

        Vector2 inputVector = GetInput();
        #region Flip
        if (inputVector.x > 0 && !facingRight)
        {
            Flip();
        }
        else if (inputVector.x < 0 && facingRight)
        {
            Flip();
        }
        #endregion 

        anim?.SetFloat("Speed", Mathf.Abs(inputVector.x));
        grounded = Grounded();
        anim?.SetBool("IsGrounded", grounded);
        //TO-DO: Check gravity if needed.        
        //inputVector.y = verticalVelocity;
        verticalVelocity = rb.velocity.y;
        anim?.SetFloat("VerticalVelocity", verticalVelocity);

        if (!isCrouching)
        {
            Vector2 direcction = new Vector2(inputVector.x, 0);
            transform.Translate(direcction * Time.deltaTime * maxSpeed);
        }

        //velocity = rb.velocity;
        //velocity = inputVector * Time.deltaTime * maxSpeed;
        //velocity.x = Mathf.Clamp(velocity.x, -maxSpeed, maxSpeed);
        //rb.velocity = velocity;
    }

    public void Crouch()
    {
        Vector2 inputVector = GetInput();
        isCrouching = inputVector.y < 0f;
        if (isCrouching) {
            anim.SetBool("IsCrouching", isCrouching);
        } else {            
            anim.SetBool("IsCrouching", isCrouching);
        }
    }

    public void Jump()
    {
        if (grounded && canJump && !isCrouching)
        {
            rb.velocity = new Vector2(rb.velocity.x, 0f);
            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
            anim?.SetTrigger("Jump");
        }
    }

    public void BetterJump()
    {
        if (rb.velocity.y < 0)
        {
            rb.velocity += Vector2.up * Physics2D.gravity.y * (fallMultiplier - 1) * Time.deltaTime;
        }
    }

    private Vector2 GetInput()
    {
        return InputController.Instance.Move.normalized;
    }
    public bool Grounded()
    {
        return Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundlayer);
    }
    private void Flip()
    {
        facingRight = !facingRight;
        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
    }

}
