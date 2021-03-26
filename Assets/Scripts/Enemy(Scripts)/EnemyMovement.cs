using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

/// <summary>
/// The responsability of this script relies exclusively on the enemy movement. 
/// This means pathfinding, jumping and moving the player
/// </summary>
public class EnemyMovement : MonoBehaviour
{

    #region pathFinding
    [HideInInspector]
    public Seeker seeker;
    private Path path;
    [Header("Pathfinding")]
    public Transform target;   
    public LayerMask groundLayer;
    [SerializeField] float pathfindOffset;

    #endregion

    [Header("Physics")]
    public float speed = 200f;
    public float maxSpeed = 10f;
    public float acceleration = 5f;
    public float nextWaypointDistance = 3f;
    public float jumpNodeHeightRequirement = 0.8f;
    public float jumpForce = 0.3f;
    public float jumpCheckOffset = 0.1f;
    public Transform groundDetection;

    public bool directionLookEnabled = true;
    
    public bool jumpEnabled = true;
    [SerializeField] float groundOffset;
    [SerializeField] float maxYVelocity;

    Animator animator;
    Rigidbody2D rb;

    private bool isGrounded = false;
    private int currentWaypoint = 0;
    private Vector2 velocity;
    private Vector2 direction;


    [SerializeField] bool CanJump;

    bool groundInfoDown = true;
    bool groundInfoFwd = false;

    public bool IsGrounded { get => isGrounded;}

    private void Awake()
    {
        animator = GetComponentInChildren<Animator>();
        rb = GetComponent<Rigidbody2D>();
        seeker = GetComponent<Seeker>();

    }

    IEnumerator Start()
    {
        yield return new WaitUntil(() => BasicCharacter.Instance != null);
        target = BasicCharacter.Instance.transform;
        UpdatePath();
    }

    public void Chase()
    {
        CheckGround();
        CheckJumpStatus();
        if (currentWaypoint < path.vectorPath.Count) {
            HorizontalMovement();
        }
        
        if (CanJump)
        {
            jump();
        }

    }


    protected virtual void HorizontalMovement()
    {
        animator.SetBool("isRunning", true);//TO-DO cambiar isrunning por la variable serializada

        direction = ((Vector2)path.vectorPath[currentWaypoint] - rb.position).normalized;



        //Calculate force Vector



        Vector2 force = (direction * (speed * 20) * acceleration * Time.deltaTime);

        force.y = 0;
        if (!CanJump)
        {
            if (groundInfoDown == false || groundInfoFwd == true)
            {
                force = Vector2.zero;
                rb.velocity = Vector2.zero;
                animator.SetBool("isRunning", false);
            }
        }
        rb.AddForce(force);
        velocity = rb.velocity;
        //Clamp X Velocity
        velocity.x = Mathf.Clamp(velocity.x, -maxSpeed, maxSpeed);
        rb.velocity = velocity;
        //Clapm Y Velocity
        if (rb.velocity.y > maxYVelocity)
        {
            velocity = rb.velocity;
            velocity.y = maxYVelocity;
            rb.velocity = velocity;
        }

        //Next Waypoint
        float distance = Vector2.Distance(rb.position, path.vectorPath[currentWaypoint]);
        if (distance < nextWaypointDistance) currentWaypoint++;
        //Sprite direction handling
        Flip();
    }
    private void Flip()
    {
        if (directionLookEnabled)
        {
            if (direction.x > 0.05f)
            {
                transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
            }
            else if (direction.x < -0.05f)
            {
                transform.localScale = new Vector3(-1f * Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
            }
        }
    }
    public void jump()
    {
       
        if (groundInfoDown == false || groundInfoFwd == true)
        {
            if (jumpEnabled && isGrounded)
            {
                jumpEnabled = false;
                if (groundInfoDown == false)
                {
                    rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);//JUMPS HOLE
                    animator.SetTrigger("Jump");
                }
                else if (direction.y > jumpNodeHeightRequirement)//JUMPS WALL
                {
                    rb.AddForce(Vector2.up * jumpForce * 2, ForceMode2D.Impulse);
                    animator.SetTrigger("Jump");
                }
            }
        }
        else if (groundInfoDown == true)
        {
            jumpEnabled = true;
        }
    }


    protected virtual void CheckGround()
    {
        Vector3 startOffset = transform.position -  new Vector3 (0, groundOffset, 0); //new Vector3(0f, GetComponent<Collider2D>().bounds.extents.y + jumpCheckOffset);
        Debug.DrawRay(startOffset, Vector3.down, Color.red, 0.05f);
        isGrounded = Physics2D.Raycast(startOffset, Vector3.down, 0.05f);
        animator.SetBool("isGround", isGrounded);
    }
    public void CheckJumpStatus()
    {
        RaycastHit2D groundInfoDownRayCast = Physics2D.Raycast(groundDetection.position, Vector2.down, 20f, groundLayer);//HOLE
        RaycastHit2D groundInfoFwdRayCast = Physics2D.Raycast(groundDetection.position, Vector2.right*transform.localScale.x, 2f, groundLayer);//WALL
        groundInfoDown = groundInfoDownRayCast.collider;
        groundInfoFwd = groundInfoFwdRayCast.collider;
        
    }


    #region pathFindingMethods
    /// <summary>
    /// Check if the conditions are ideal to calculate a new path
    /// </summary>
    public void UpdatePath()
    {
        if (seeker.IsDone())
        {
            seeker.StartPath(rb.position + new Vector2(0, pathfindOffset), target.position, OnPathComplete);
        }
    }

    /// <summary>
    /// Method is executed as a callback once a path has finished calculating
    /// It Resets the currentWaypoint
    /// </summary>
    /// <param name="p">New Calculated Path</param>
    protected void OnPathComplete(Path p)
    {
        if (!p.error)
        {
            path = p;
            currentWaypoint = 0;

        }
    }
    #endregion


    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(groundDetection.position, (Vector2)groundDetection.position + Vector2.down * 20);
        Gizmos.color = Color.cyan;
        Gizmos.DrawLine(groundDetection.position, (Vector2)groundDetection.position + Vector2.right * transform.localScale.x * 2f);
    }

}

