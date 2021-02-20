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
    private float verticalVelocity;
    private Rigidbody2D rb;
    Vector2 velocity;
    #endregion
    #region SerializedVariables
    [SerializeField] private float groundCheckRadius = 0.2f;
    [SerializeField] private LayerMask groundlayer;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private float speedX = 5f;
    [SerializeField] private float speedY = 5f;
    [SerializeField] private float acceleration = 5f;
    [SerializeField] private float maxSpeed = 5f;
    [SerializeField] private float gravity = 0.25f;
    [SerializeField] private float terminalVelocity = 5f;
    [SerializeField] private float jumpForce = 8f;
    #endregion
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        controller = GetComponent<CharacterController>();
        anim = transform.GetChild(0).GetComponent<Animator>();
    }

    public void Move()
    {
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

        anim?.SetFloat("Speed", inputVector.magnitude);
        grounded = Grounded();
        anim?.SetBool("IsGrounded", grounded);
        //TO-DO: Check gravity if needed.        
        //inputVector.y = verticalVelocity;
        verticalVelocity = rb.velocity.y;
        anim?.SetFloat("VerticalVelocity", verticalVelocity);
        velocity = rb.velocity;
        velocity += inputVector * acceleration * Time.deltaTime;
        velocity.x = Mathf.Clamp(velocity.x, -maxSpeed, maxSpeed);
        rb.velocity = velocity;
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
        if (grounded)
        {
            rb.velocity = new Vector2(rb.velocity.x, 0f);
            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
            anim?.SetTrigger("Jump");
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
