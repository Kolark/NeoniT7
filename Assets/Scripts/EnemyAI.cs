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

    private Path path;
    private int currentWaypoint = 0;
    bool isGrounded = false;
    EnemyState state;
    EnemyController enemy; 
    Seeker seeker; 
    Rigidbody2D rb;
    Vector2 velocity;
    Animator animator;
    float t;
    float nextAttackTime;

    public EnemyState State { get => state; set => state = value; }

    void Start() {
        animator = GetComponentInChildren<Animator>();
        enemy = GetComponent<EnemyController>();
        seeker = GetComponent<Seeker>();
        rb = GetComponent<Rigidbody2D>();
    }

    void FixedUpdate() {
        PathfindingLogic();
        Debug.Log("In distance: " + TargetInDistance());
        Debug.Log("Enemy AI State: " + state);
    }

    void UpdatePath() {
        if (TargetInDistance() && followEnabled && seeker.IsDone()) {
            seeker.StartPath(rb.position, target.position, OnPathComplete);
        }
    }

    public virtual void PathfindingLogic() {
        switch (state) {
            case EnemyState.Idle:
                animator.SetBool("isRunning", false);
                if (TargetInDistance() && followEnabled) {
                    InvokeRepeating("UpdatePath", 0f, pathUpdateSeconds);
                    state = EnemyState.Chase;
                }
                else seeker.enabled = false;
                break;
            case EnemyState.Chase:
                if (path == null) return;
                if (currentWaypoint >= path.vectorPath.Count) {
                    return;
                }
                animator.SetBool("isRunning", true);

                //See if colliding with anything
                Vector3 startOffset = transform.position - new Vector3(0f, GetComponent<Collider2D>().bounds.extents.y + jumpCheckOffset);
                isGrounded = Physics2D.Raycast(startOffset, Vector3.down, 0.05f);
                Debug.DrawRay(startOffset, Vector3.down, Color.red, 0.05f);
                Debug.Log("Grounded: " + isGrounded);

                //Direction Calculation
                Vector2 direction = ((Vector2)path.vectorPath[currentWaypoint] - rb.position).normalized;
                Vector2 force = (direction * (speed*20) * acceleration * Time.deltaTime);

                //Movement
                rb.AddForce(force);
                velocity = rb.velocity;
                velocity.x = Mathf.Clamp(velocity.x, -maxSpeed, maxSpeed);
                rb.velocity = velocity;
                //if (isGrounded) rb.AddForce(force);

                //Check for a hole in the ground and jump.
                RaycastHit2D groundInfoDown = Physics2D.Raycast(groundDetection.position, Vector2.down, 6f, groundLayer);
                RaycastHit2D groundInfoFwd = Physics2D.Raycast(groundDetection.position, Vector2.right, 0.2f, groundLayer);

                if (groundInfoDown.collider == false || groundInfoFwd.collider == true) {
                    if (jumpEnabled && isGrounded) {
                        rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
                        Debug.Log("Jumpin"); 
                        if (direction.y > jumpNodeHeightRequirement) rb.AddForce(Vector2.up * jumpForce * 2, ForceMode2D.Impulse);;
                        //if (direction.y > jumpNodeHeightRequirement) 
                    }                    
                }

                //Next Waypoint
                float distance = Vector2.Distance(rb.position, path.vectorPath[currentWaypoint]);
                if (distance < nextWaypointDistance) currentWaypoint++;

                //Check attack availability
                float distanceToTarget = 0;
                if (target!= null) distanceToTarget = Vector2.Distance(transform.position, target.position);
                if (distanceToTarget < enemy.AttackDistance) {
                    rb.velocity = Vector2.zero;
                    state = EnemyState.Attack;
                }

                //Sprite direction handling
                if (directionLookEnabled) {
                    if (rb.velocity.x > 0.05f) {
                        transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
                    }
                    else if (rb.velocity.x < -0.05f) {
                        transform.localScale = new Vector3(-1f * Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
                    }
                }

                //TO-DO:
                // 1. Animations
                // 2. Transition to other states.
                // 3. Cancel Path if falling
                // 4. Check between waypoints to see if attack is available.

                //animator.SetBool("IsWalking", true); // Some sort of walking animation.(WIP)
                break;
            case EnemyState.Attack:                                   
                if (Time.time >= nextAttackTime) {
                    animator.SetBool("isAttacking", true);
                    animator.SetBool("isRunning", false);
                    //animator.SetBool("isAttacking", true); 
                    enemy.Attack();
                    nextAttackTime = Time.time + 1f / attackRate;
                }
                if (target == null) state = EnemyState.Idle;
                else state = EnemyState.Chase;
                //TO-DO: Attack logic goes here.
                break;
            case EnemyState.Dead:
                //TO-DO: Death logic goes here.
                break;
            case EnemyState.BackToStart:
                //TO-DO: For patrolling or losing player focus, going back to the start logic goes here.
                break;
        }
    }    

    private bool TargetInDistance() {
        if (target != null) {
            return Vector2.Distance(transform.position, target.transform.position) < activateDistance;
        } else return false;       
    }

    private void OnPathComplete(Path p) {
        if (!p.error) {
            path = p;
            currentWaypoint = 0;
            Debug.Log("Path completed");
        }
    }



}
