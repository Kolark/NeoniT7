using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public enum EnemyState {Idle, Chase, Attack, Dead, BackToStart}

public class EnemyAI : MonoBehaviour
{
    [Header("Pathfinding")]
    public Transform target;
    public float activateDistance = 50f;
    public float pathUpdateSeconds = 0.5f;
    public LayerMask groundLayer;

    [Header("Physics")]
    public float speed = 200f;
    public float maxSpeed = 10f;
    public float acceleration = 5f;
    public float nextWaypointDistance = 3f;
    public float jumpNodeHeightRequirement = 0.8f;
    public float jumpForce = 0.3f;
    public float jumpCheckOffset = 0.1f;
    public Transform groundDetection;

    [Header("Custom Behavior")]
    public bool followEnabled = true;
    public bool jumpEnabled = true;
    public bool directionLookEnabled = true;
    public bool attackEnabled = true;
    [SerializeField] float attackRate = 2f;
    [SerializeField] float  maxYVelocity;
    private Path path;
    //Reads al the points in path
    private int currentWaypoint = 0;
    bool isGrounded = false;
    EnemyState state;
    EnemyController enemy; 
    Seeker seeker; 
    Rigidbody2D rb;
    Vector2 velocity;
    Animator animator;
    float timer = 0;
    float nextAttackTime;

    bool canAttack = true;


    public EnemyState State { get => state; set => state = value; }
    private void Awake()
    {
        animator = GetComponentInChildren<Animator>();
        enemy = GetComponent<EnemyController>();
        seeker = GetComponent<Seeker>();
        rb = GetComponent<Rigidbody2D>();
    }
    void Start() {
        target = BasicCharacter.Instance.transform;
    }

    //void FixedUpdate() {
    //    PathfindingLogic();
    //    //Debug.Log("In distance: " + TargetInDistance());
    //    //Debug.Log("Enemy AI State: " + state);
    //}
    

    public virtual void PathfindingLogic() {
        switch (state) {
            #region IDLE
            case EnemyState.Idle:
                Idle();
                break;
            #endregion
            #region CHASE
            case EnemyState.Chase:
                if (path == null) return;
                if (currentWaypoint >= path.vectorPath.Count) { return; }
                Chase();
                break;
            #endregion
            #region ATTACK
            case EnemyState.Attack:
                Attack();
                break;
            #endregion
            #region DEAD
            case EnemyState.Dead:
                //TO-DO: Death logic goes here.
                break;
            #endregion
            #region BACKTOSTART
            case EnemyState.BackToStart:
                //TO-DO: For patrolling or losing player focus, going back to the start logic goes here.
                break;
            #endregion
        }
    }    

    protected virtual void Idle()
    {
        animator.SetBool("isRunning", false);
        //If in distance start chase
        if (TargetInDistance() && followEnabled)
        {
            InvokeRepeating("UpdatePath", 0f, pathUpdateSeconds);
            state = EnemyState.Chase;
        }
        else seeker.enabled = false;
    }
    protected virtual void Chase()
    {
        //Responsabilities: Follow player, jump, decide whether to attack or not
        animator.SetBool("isRunning", true);

        //See if colliding with anything
        isGrounded = checkGround();
        //Debug.Log("Grounded: " + isGrounded);

        //Direction Calculation
        Vector2 direction = ((Vector2)path.vectorPath[currentWaypoint] - rb.position).normalized;
        //Vector2 force = (direction * (speed * 20) * acceleration * Time.deltaTime);//OLD
        Vector2 force = (direction * (speed * 20) * acceleration * Time.deltaTime);//new
        force.y = 0;                                                               //new
        //Movement
        rb.AddForce(force);
        velocity = rb.velocity;
        velocity.x = Mathf.Clamp(velocity.x, -maxSpeed, maxSpeed);
        rb.velocity = velocity;
        //if (isGrounded) rb.AddForce(force);

        //Check for a hole in the ground and jump.
        RaycastHit2D groundInfoDown = Physics2D.Raycast(groundDetection.position, Vector2.down, 6f, groundLayer);//HOLE
        RaycastHit2D groundInfoFwd = Physics2D.Raycast(groundDetection.position, Vector2.right, 0.5f, groundLayer);//WALL

        if (groundInfoDown.collider == false || groundInfoFwd.collider == true)
        {
            if (jumpEnabled && isGrounded)
            {
                jumpEnabled = false;
                if(groundInfoDown.collider == false)
                {
                    rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);//JUMPS HOLE
                }
                else if (direction.y > jumpNodeHeightRequirement)//JUMPS WALL
                {
                    rb.AddForce(Vector2.up * jumpForce* 2, ForceMode2D.Impulse);
                    //Debug.Log("Jumpin WALL");
                }
            }
        }
        else if(groundInfoDown.collider == true)
        {
            jumpEnabled = true;
        }

        if(rb.velocity.y > maxYVelocity){
            velocity = rb.velocity;
            velocity.y = maxYVelocity;
            rb.velocity = velocity;
        }
        
        

        //Next Waypoint
        float distance = Vector2.Distance(rb.position, path.vectorPath[currentWaypoint]);
        if (distance < nextWaypointDistance) currentWaypoint++;

        //Check attack availability
        float distanceToTarget = 0;
        if (target != null) distanceToTarget = Vector2.Distance(transform.position, target.position);

        timer += Time.fixedDeltaTime;
        if (distanceToTarget < enemy.AttackDistance)//within distance
        {
            if (timer >= attackRate)
            {
                rb.velocity = Vector2.zero;
                state = EnemyState.Attack;
                timer = 0;
            }
        }
        



        //Sprite direction handling
        Flip();

        //TO-DO:
        // 1. Animations
        // 2. Transition to other states.
        // 3. Cancel Path if falling
        // 4. Check between waypoints to see if attack is available.

        //animator.SetBool("IsWalking", true); // Some sort of walking animation.(WIP)
    }
    private void Flip()
    {
        if (directionLookEnabled)
        {
            if (rb.velocity.x > 0.05f)
            {
                transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
            }
            else if (rb.velocity.x < -0.05f)
            {
                transform.localScale = new Vector3(-1f * Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
            }
        }
    }
    protected virtual bool checkGround()
    {
        Vector3 startOffset = transform.position - new Vector3(0f, GetComponent<Collider2D>().bounds.extents.y + jumpCheckOffset);
        Debug.DrawRay(startOffset, Vector3.down, Color.red, 0.05f);
        return Physics2D.Raycast(startOffset, Vector3.down, 0.05f);
    }

    protected virtual void Attack()
    {
        if (canAttack)
        {
            animator.SetTrigger("Attack");
            enemy.Attack();
            canAttack = false;
        }
    }

    public virtual void onAttackEnd()
    {
        canAttack = true;
        if (target == null) state = EnemyState.Idle;
        else state = EnemyState.Chase;
    }

    /// <summary>
    /// Checks if targe is in range
    /// </summary>
    /// <returns></returns>
    private bool TargetInDistance() {
        if (target != null) {
            return Vector2.Distance(transform.position, target.transform.position) < activateDistance;
        } else return false;       
    }

    /// <summary>
    /// Check if the conditions are ideal to calculate a new path
    /// </summary>
    void UpdatePath()
    {
        if (TargetInDistance() && followEnabled && seeker.IsDone())
        {
            seeker.StartPath(rb.position, target.position, OnPathComplete);
        }
    }

    /// <summary>
    /// Method is executed as a callback once a path has finished calculating
    /// It Resets the currentWaypoint
    /// </summary>
    /// <param name="p">New Calculated Path</param>
    private void OnPathComplete(Path p) {
        if (!p.error) {
            path = p;
            currentWaypoint = 0;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(groundDetection.position, (Vector2)groundDetection.position + Vector2.down*6);
        Gizmos.color = Color.cyan;
        Gizmos.DrawLine(groundDetection.position, (Vector2)groundDetection.position + Vector2.right*1f);
    }


}
