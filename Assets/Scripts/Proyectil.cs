using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Proyectil : MonoBehaviour
{
    Rigidbody2D rb2d;
    bool used = false;
    public void push(Vector2 dir)
    {
        rb2d = GetComponent<Rigidbody2D>();
        rb2d.AddForce(dir*25,ForceMode2D.Impulse);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!used)
        {
            IEnemyHurtBox enemy = collision?.GetComponent<IEnemyHurtBox>();
            enemy?.OnReceiveDamage();
            Destroy(this.gameObject);
            used = true;
        }

    }
}
