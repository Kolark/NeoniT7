using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OniBProjectile : MonoBehaviour
{
    [SerializeField] float lifeTime;

    void Start() {
        Invoke("DestroyProjectile", lifeTime);
    }

    private void OnCollisionEnter2D(Collision2D other) {
        Debug.Log("Colision con" + LayerMask.LayerToName(other.gameObject.layer));
        if (other.gameObject.CompareTag("Player")) { //Piratada necesaria
            Rigidbody2D rb = other.gameObject.GetComponent<Rigidbody2D>();
            PlayerDamageHandler player = other.gameObject.GetComponentInChildren<PlayerDamageHandler>();
            if (player != null)  {
                player.OnReceiveDamage();
                rb.velocity = new Vector2(0f, rb.velocity.y);
                Destroy(gameObject); //TO-DO (?): Implement pool 
            }
        }
    }

    void DestroyProjectile() {
        Destroy(gameObject); //TO-DO (?): Implement pool 
    }
}
