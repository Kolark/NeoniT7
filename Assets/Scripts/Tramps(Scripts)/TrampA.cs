using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrampA : MonoBehaviour
{
    [SerializeField] Transform pos;
    [SerializeField] Vector3 size;
    [SerializeField] LayerMask layer;
    [SerializeField] float damageTime, releseTime;
    Animator animator;

    private void Awake()
    {
        animator = gameObject.GetComponent<Animator>();
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        
        if (collision.gameObject.tag == "Player")
        {
            ActiveTramp();
        }
    }

    public void ActiveTramp()
    {
        animator.SetTrigger("Activate");
        Invoke("Damage", damageTime);
    }

    void Damage()
    {
        Collider2D Hit = Physics2D.OverlapBox(pos.position, size, 0f, layer);
        PlayerDamageHandler player = Hit?.GetComponent<PlayerDamageHandler>();
        player?.Death();
        Invoke("Release", releseTime);
    }

    public void Release()
    {
        animator.SetTrigger("Release");
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(pos.position, size);
    }
}
