using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyGround : MonoBehaviour
{
    [SerializeField] EnemyMovement enemyMovement;
    private void OnTriggerEnter2D(Collider2D other) {
        enemyMovement.IsGrounded = other.gameObject.layer == LayerMask.NameToLayer("Ground") ? true : false; 
    }

    private void OnTriggerExit2D(Collider2D other) {
        enemyMovement.IsGrounded = other.gameObject.layer == LayerMask.NameToLayer("Ground") ? false : true;
    }

    private void OnTriggerStay2D(Collider2D other) {
        
    }
}
