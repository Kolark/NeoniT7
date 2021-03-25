using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathZone : MonoBehaviour
{
    [SerializeField] Vector3 size;
    [SerializeField] LayerMask layer;

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            Collider2D Hit = Physics2D.OverlapBox(transform.position, size, 0f, layer);
            PlayerDamageHandler player = Hit?.GetComponent<PlayerDamageHandler>();
            player?.Death();
        }

        if (collision.gameObject.GetComponent<EnemyController>() != null)
        {
            ///DEATH
        }

    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(transform.position, size);
    }

}
