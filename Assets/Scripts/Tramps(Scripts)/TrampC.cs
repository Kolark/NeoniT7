using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrampC : MonoBehaviour
{
    [SerializeField] Vector3 size;
    [SerializeField] LayerMask layer;
    [SerializeField] float releseTime;
    [SerializeField] GameObject col;
    [SerializeField] bool active = false, objectActive;
    [SerializeField] Animator animator;
    float time = 0;

    private void Start()
    {
        active = objectActive;
        animator = animator.GetComponent<Animator>();
    }

    private void Update()
    {
        time += Time.deltaTime;
        if (active)
        {
            ActiveTramp();
        }
        else
        {
            DesactiveTramp();
        }

        if(time>=releseTime && active)
        {
            animator.SetBool("Active", false);
            time = 0;
            active = false;
        }else if(time >= releseTime && !active)
        {
            animator.SetBool("Active", true);
            time = 0;
            active = true;
        }
    }

    public void ActiveTramp()
    {
        Collider2D Hit = Physics2D.OverlapBox(col.transform.position, size, 0f, layer);
        PlayerDamageHandler player = Hit?.GetComponent<PlayerDamageHandler>();
        player?.OnReceiveDamage();
        if (!objectActive)
        {
            col.gameObject.SetActive(true);
            objectActive = true;
        }
    }

    public void DesactiveTramp()
    {
        if (objectActive)
        {
            col.gameObject.SetActive(false);
            objectActive = false;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(col.transform.position, size);
    }
}
