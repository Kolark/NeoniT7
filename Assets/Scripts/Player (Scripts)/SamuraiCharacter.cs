using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class SamuraiCharacter : BasicCharacter
{
    
    ///TO-DO sincronizar el ataque del ultimate con la animacion

    bool isParry = false;
    [SerializeField] GameObject projectil;
    [SerializeField] AttackInfo specialAttack;
    public override void Defense()
    {
        base.Defense();
    }

    public override void StartParry()
    {
        canReceiveDamage = false;
        isParry = true;
    }
    public override void EndParry()
    {
        isParry = false;
        canReceiveDamage = true;
    }
    public override void Throwable()
    {
        base.Throwable();
        GameObject gameObject = Instantiate(projectil,firstAttack.pos.position,Quaternion.identity);
        Proyectil proyectil = gameObject.GetComponent<Proyectil>();
        proyectil.push(Vector2.right * transform.localScale.x);
    }
    public override void Ultimate()
    {
        base.Ultimate();
        Collider2D[] Hit = Physics2D.OverlapCircleAll(specialAttack.pos.position, specialAttack.radius, specialAttack.layer);
        for (int i = 0; i < Hit.Length; i++)
        {
            IEnemyHurtBox enemy = Hit[i]?.GetComponent<IEnemyHurtBox>();
            enemy?.OnReceiveDamage();
        }
    }
    public override void Damage()
    {
        if (canReceiveDamage)
        {
            maxLife--;
            bool isDead = maxLife <= 0;
            if (isDead)
            {
                isAlive = false;
                character.Anim.SetTrigger("Death");
            }
        }
        else if(isParry)
        {
            //
            Counter();
            isParry = false;
            canReceiveDamage = true;
        }
    }
    public override void Counter()
    {
        character.Anim.SetTrigger("Counter");
        Collider2D[] Hit = Physics2D.OverlapCircleAll(counter.pos.position, counter.radius, counter.layer);
        for (int i = 0; i < Hit.Length; i++)
        {
            IEnemyHurtBox enemy = Hit[i]?.GetComponent<IEnemyHurtBox>();
            enemy?.OnReceiveDamage();
        }
    }
    
}
