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
    private Animator anim;
    private CharacterController controller;
    private Vector3 slopeNormal;
    private bool grounded;
    private bool facingRight = true;
    private bool isCrouching;
    private float verticalVelocity;
    private Rigidbody2D rb;
    Vector2 velocity;

    InputController inputController;
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

    [SerializeField] private float extremitiesOffset = 0.05f;
    [SerializeField] private float innerVerticalOffset = 0.25f;
    [SerializeField] private float distanceGrounded = 0.15f;
    [SerializeField] private float slopeThreshold = 0.55f;
    #endregion
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        controller = GetComponent<CharacterController>();
        anim = transform.GetChild(0).GetComponent<Animator>();
    }

    private void Start()
    {
        inputController = InputController.Instance;
        inputController.Jump += Jump;
        //inputController.Attack += Crouch;//Nombre temporal mientras se agrega el input de Crouch
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
        print(grounded);
        anim?.SetBool("Grounded", grounded);
        //TO-DO: Check gravity if needed.        
        //inputVector.y = verticalVelocity;
        anim?.SetFloat("VerticalVelocity", verticalVelocity);

        //if (slopeNormal != Vector3.up) moveVector = FollowFloor(moveVector);

        
        velocity = rb.velocity;
        velocity += inputVector * acceleration * Time.deltaTime;
        velocity.x = Mathf.Clamp(velocity.x, -maxSpeed, maxSpeed);
        rb.velocity = velocity;
    }


    public void Crouch()
    {
        Vector2 inputVector = GetInput();
        print(inputVector);
        isCrouching = inputVector.y < 0f;
        if (isCrouching) {
            Debug.Log("Ma men do be crouching");
            anim.SetBool("IsCrouching", isCrouching);
        } else {            
            Debug.Log("Not crouching");
            anim.SetBool("IsCrouching", isCrouching);
        }
    }

    private void Jump()
    {
        if (grounded)
        {
            rb.velocity = new Vector2(rb.velocity.x, 0f);
            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
            // verticalVelocity = jumpForce;
            // slopeNormal = Vector3.up;
            anim?.SetTrigger("Jump");
        }
    }
    private Vector2 GetInput()
    {
        Vector2 r = inputController.Move;
        //Vector3 r = Vector3.zero;
        //r.x = inputController.Move.x;
        //r.y = inputController.Move.y;

        return r.normalized;
    }
    // private Vector3 FollowFloor(Vector3 _moveVector)
    // {
    //     Vector3 right = new Vector3(slopeNormal.y, -slopeNormal.x, 0).normalized;
    //     Vector3 forward = new Vector3(0, -slopeNormal.z, slopeNormal.y).normalized;
    //     return right * _moveVector.x + forward * _moveVector.z;
    // }
    public bool Grounded()
    {
        return Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundlayer);
        // if (verticalVelocity > 0)
        //     return false;

        // float yRay = (controller.bounds.center.y - (controller.height * 0.5f)) + innerVerticalOffset;

        // RaycastHit hit;
        // //Middle
        // if (Physics.Raycast(new Vector3(controller.bounds.center.x, yRay, controller.bounds.center.z), -Vector3.up, out hit, innerVerticalOffset + distanceGrounded))
        // {
        //     Debug.DrawRay(new Vector3(controller.bounds.center.x, yRay, controller.bounds.center.z), -Vector3.up * (innerVerticalOffset + distanceGrounded), Color.red);
        //     slopeNormal = hit.normal;
        //     return (slopeNormal.y > slopeThreshold) ? true : false;
        // }
        // //Front Right
        // if (Physics.Raycast(new Vector3(controller.bounds.center.x + (controller.bounds.extents.x - extremitiesOffset), yRay, controller.bounds.center.z + (controller.bounds.extents.z - extremitiesOffset)), -Vector3.up, out hit, innerVerticalOffset + distanceGrounded))
        // {
        //     slopeNormal = hit.normal;
        //     return (slopeNormal.y > slopeThreshold) ? true : false;
        // }
        // //Front Left
        // if (Physics.Raycast(new Vector3(controller.bounds.center.x - (controller.bounds.extents.x - extremitiesOffset), yRay, controller.bounds.center.z + (controller.bounds.extents.z - extremitiesOffset)), -Vector3.up, out hit, innerVerticalOffset + distanceGrounded))
        // {
        //     slopeNormal = hit.normal;
        //     return (slopeNormal.y > slopeThreshold) ? true : false;
        // }
        // //Back Right
        // if (Physics.Raycast(new Vector3(controller.bounds.center.x + (controller.bounds.extents.x - extremitiesOffset), yRay, controller.bounds.center.z - (controller.bounds.extents.z - extremitiesOffset)), -Vector3.up, out hit, innerVerticalOffset + distanceGrounded))
        // {
        //     slopeNormal = hit.normal;
        //     return (slopeNormal.y > slopeThreshold) ? true : false;
        // }
        // //Back Left
        // if (Physics.Raycast(new Vector3(controller.bounds.center.x - (controller.bounds.extents.x - extremitiesOffset), yRay, controller.bounds.center.z - (controller.bounds.extents.z - extremitiesOffset)), -Vector3.up, out hit, innerVerticalOffset + distanceGrounded))
        // {
        //     slopeNormal = hit.normal;
        //     return (slopeNormal.y > slopeThreshold) ? true : false;
        // }

        //return false;
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
