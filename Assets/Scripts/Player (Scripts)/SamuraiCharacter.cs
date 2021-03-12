using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class SamuraiCharacter : BasicCharacter
{

    ///TO-DO sincronizar el ataque del ultimate con la animacion
    [SerializeField] Animator ultimateAnim;
    bool isParry = false;
    [SerializeField] GameObject projectil;
    [SerializeField] AttackInfo specialAttack;
    [SerializeField] float ultimateOffsetTime;
    public override void Defense()
    {
        if (!isAlive) return;
        if (!canUseDefense) return;
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
        if (!isAlive) return;
        if (!canUseThrowable) return;
        if (!character.Grounded) return;
        base.Throwable();
        GameObject gameObject = Instantiate(projectil,firstAttack.pos.position,Quaternion.identity);
        Proyectil proyectil = gameObject.GetComponent<Proyectil>();
        proyectil.push(Vector2.right * transform.localScale.x);
    }
    public override void Ultimate()
    {
        if (!isAlive) return;
        if (!canUseSpecial) return;
        if (!character.Grounded) return;
        base.Ultimate();
        ultimateAnim.SetTrigger("Ultimate");
        DOVirtual.DelayedCall(ultimateOffsetTime,()=> {
            Collider2D[] Hit = Physics2D.OverlapCircleAll(specialAttack.pos.position, specialAttack.radius, specialAttack.layer);
            for (int i = 0; i < Hit.Length; i++)
            {
                IEnemyHurtBox enemy = Hit[i]?.GetComponent<IEnemyHurtBox>();
                enemy?.OnReceiveDamage();
            }
        });


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
                MenuManager.Instance.Pause();
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
    protected override void OnDrawGizmos()
    {
        base.OnDrawGizmos();
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(specialAttack.pos.position, specialAttack.radius);

    }
}
