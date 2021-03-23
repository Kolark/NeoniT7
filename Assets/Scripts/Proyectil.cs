using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Proyectil : MonoBehaviour
{
    Rigidbody2D rb2d;
    bool used = false;
    [SerializeField] float timeAlive;
    [SerializeField] int hitsNumber;
    public void push(Vector2 dir)
    {
        bool scaledir = dir.x > 0;

        if (!scaledir)
        {
            transform.localScale = new Vector2(transform.localScale.x*-1,transform.localScale.y);
        }

        rb2d = GetComponent<Rigidbody2D>();
        rb2d.AddForce(dir*25,ForceMode2D.Impulse);
        DG.Tweening.DOVirtual.DelayedCall(timeAlive, () => { Destroy(this.gameObject); });
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
            IEnemyHurtBox enemy = collision?.GetComponent<IEnemyHurtBox>();
            
        if(enemy != null)
        {
            for (int i = 0; i < hitsNumber; i++)//cambiarlo a que en vesd e un for sea un parametro del recibir daño
            {
                enemy?.OnReceiveDamage();
            }
        }
            
    }
}
