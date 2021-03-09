using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class YakuzaCharacter : BasicCharacter
{
    bool canTankDamage = false;
    [SerializeField] GameObject projectil;
    [SerializeField] AttackInfo specialAttack;
    [SerializeField] AttackInfo explosiveinfo;
    public override void Defense()
    {
        if (!isAlive) return;
        if (!canUseDefense) return;
        base.Defense();
        Collider2D[] Hit = Physics2D.OverlapCircleAll(specialAttack.pos.position, specialAttack.radius, specialAttack.layer);
        for (int i = 0; i < Hit.Length; i++)
        {

            //Aca va
            Rigidbody2D rb2d = Hit[i].GetComponent<Rigidbody2D>();
            rb2d.AddForce((rb2d.transform.position - transform.position).normalized * 4, ForceMode2D.Impulse);
            //IEnemyHurtBox enemy = Hit[i]?.GetComponent<IEnemyHurtBox>();
            //enemy?.OnReceiveDamage();
        }
    }

    public override void StartParry()
    {
        canReceiveDamage = false;
        canTankDamage = true;
    }
    public override void EndParry()
    {
        canTankDamage = false;
        canReceiveDamage = true;
    }
    public override void Throwable()
    {
        if (!isAlive) return;
        if (!canUseThrowable) return;
        if (!character.Grounded) return;
        base.Throwable();
        GameObject gameObject = Instantiate(projectil, firstAttack.pos.position, Quaternion.identity);
        Proyectil proyectil = gameObject.GetComponent<Proyectil>();
        proyectil.push(Vector2.right * transform.localScale.x);
    }
    public override void Ultimate()
    {
        if (!isAlive) return;
        if (!canUseSpecial) return;
        if (!character.Grounded) return;
        base.Ultimate();
///Aca va la ultimate
    }
    public override void Damage()
    {
        Debug.Log("Attack step 5");
        if (canReceiveDamage)
        {
            maxLife--;
            Debug.Log("Attack step 6");
            bool isDead = maxLife <= 0;
            if (isDead)
            {
                isAlive = false;
                canReceiveDamage = false;
                character.Anim.SetTrigger("Death");
            }
        }
        else if (canTankDamage)
        {
            //
            canTankDamage = false;
            //Shield Break
            canReceiveDamage = true;
        }
    }

}
