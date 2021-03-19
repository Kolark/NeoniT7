using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrampA : MonoBehaviour
{
    [SerializeField] Transform pos;
    [SerializeField] Vector3 size;
    [SerializeField] LayerMask layer;
    [SerializeField] float releseTime;
    [SerializeField] SpriteRenderer col;
    Color previousCol;

    public void OnTriggerEnter2D(Collider2D collision)
    {
        
        if (collision.gameObject.tag == "Player")
        {
            
            Invoke("ActiveTramp", releseTime);
        }
    }

    public void ActiveTramp()
    {
        Collider2D Hit = Physics2D.OverlapBox(pos.position, size, 0f, layer);
        PlayerDamageHandler player = Hit?.GetComponent<PlayerDamageHandler>();
        player?.OnReceiveDamage();
        previousCol = col.color;
        col.color = Color.red;
        Invoke("Release", releseTime);
    }

    public void Release()
    {
        col.color = previousCol;
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(pos.position, size);
    }
}
