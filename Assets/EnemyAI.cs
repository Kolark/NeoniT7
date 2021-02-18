﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class EnemyAI : MonoBehaviour
{
    [Header("Pathfinding")]
    public Transform target;
    public float activateDistance = 50f;
    public float pathUpdateSeconds = 0.5f;
    public LayerMask groundLayer;

    [Header("Physics")]
    public float speed = 200f;
    public float nextWaypointDistance = 3f;
    public float jumpNodeHeightRequirement = 0.8f;
    public float jumpModifier = 0.3f;
    public float jumpCheckOffset = 0.1f;

    [Header("Custom Behavior")]
    public bool followEnabled = true;
    public bool jumpEnabled = true;
    public bool directionLookEnabled = true;

    private Path path;
    private int currentWaypoint = 0;
    bool isGrounded = false;
    Seeker seeker; 
    Rigidbody2D rb;

    void Start() {
        seeker = GetComponent<Seeker>();
        rb = GetComponent<Rigidbody2D>();
        InvokeRepeating("UpdatePath", 0f, pathUpdateSeconds);
    }

    void FixedUpdate() {
        if (TargetInDistance() && followEnabled) {
            PathFollow();
        }
    }

    void UpdatePath() {
        if (TargetInDistance() && followEnabled && seeker.IsDone()) {
            seeker.StartPath(rb.position, target.position, OnPathComplete);
        }
    }

    void PathFollow() {
        if (path == null) return;
        if (currentWaypoint >= path.vectorPath.Count) return;

        //See if colliding with anything
        Vector3 startOffset = transform.position - new Vector3(0f, GetComponent<Collider2D>().bounds.extents.y + jumpCheckOffset);
        isGrounded = Physics2D.Raycast(startOffset, Vector3.down, 0.05f);
        Debug.DrawRay(startOffset, Vector3.down, Color.red, 0.05f);
        Debug.Log("Grounded: " + isGrounded);
        
        // isGrounded = Physics2D.Raycast(transform.position, Vector2.down, 0.2f + jumpCheckOffset, groundLayer);

        //Direction Calculation
        Vector2 direction = ((Vector2)path.vectorPath[currentWaypoint] - rb.position).normalized;
        Vector2 force = direction * speed * Time.deltaTime;

        //Jump
        //TO-DO: Check for holes in front of the enemy and make him jump
        //TO-DO: Check in front of enemy to see if against a wall (make jump more precise)
        if (jumpEnabled && isGrounded) {
            if (direction.y > jumpNodeHeightRequirement) rb.AddForce(Vector2.up * speed * jumpModifier); 
        }

        //Movement
        rb.AddForce(force);

        //Next Waypoint
        float distance = Vector2.Distance(rb.position, path.vectorPath[currentWaypoint]);
        if (distance < nextWaypointDistance) currentWaypoint++;

        //Sprite direction handling
        if (directionLookEnabled) {
            if (rb.velocity.x > 0.05f) {
                transform.localScale = new Vector3(-1f * Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
            }
            else if (rb.velocity.x < -0.05f) {
                transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
            }
        }            
    }

    private bool TargetInDistance() {
        return Vector2.Distance(transform.position, target.transform.position) < activateDistance;
    }

    private void OnPathComplete(Path p) {
        if (!p.error) {
            path = p;
            currentWaypoint = 0;
        }
    }



}
