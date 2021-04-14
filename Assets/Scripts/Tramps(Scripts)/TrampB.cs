using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrampB : MonoBehaviour
{
    [SerializeField] Vector3 size;
    [SerializeField] LayerMask layer;
    [SerializeField] Transform pointA, pointB;
    [SerializeField] float speed;
    Transform point;
    [SerializeField]
    float distance;

    private void Start()
    {
        point = pointA;
    }

    private void Update()
    {
        distance = Mathf.Abs(point.position.x - transform.position.x);
        Vector2 direction = point.position - transform.position;
        direction.Normalize();
        transform.Translate(direction * Time.deltaTime * speed);
        if (distance <= 0.1 && point == pointA)
        {
            point = pointB;
        }
        else if (distance <= 0.1 && point == pointB)
        {
            point = pointA;
        }
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            Collider2D Hit = Physics2D.OverlapBox(transform.position, size, 0f, layer);
            PlayerDamageHandler player = Hit?.GetComponent<PlayerDamageHandler>();
            player?.OnReceiveDamage();
        }

        if (collision.gameObject.GetComponent<EnemyController>() != null)
        {
            collision.gameObject.GetComponent<EnemyController>().Death();
            ///DEATH
        }

    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(transform.position, size);
    }
}
