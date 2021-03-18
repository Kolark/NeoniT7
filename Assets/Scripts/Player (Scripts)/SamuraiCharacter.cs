﻿using System.Collections;
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
    [SerializeField] float projectileDelay =.45f;
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
        effectsModule.PlayEffect((int)effectsSamurai.startParry);
        Debug.Log("StartParry");
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
        DOVirtual.DelayedCall(projectileDelay, null, true).OnComplete(() =>
        {
            ProjectileFlip();
            GameObject gameObject = Instantiate(projectil, firstAttack.pos.position, Quaternion.identity);
            Proyectil proyectil = gameObject.GetComponent<Proyectil>();
            proyectil.push(Vector2.right * transform.localScale.x);
        });
    }
    public override void Ultimate()
    {
        if (!isAlive) return;
        if (!canUseSpecial) return;
        if (!character.Grounded) return;
        base.Ultimate();
        effectsModule.StopEffect((int)effectsSamurai.UltReady);
        DOVirtual.DelayedCall(0.3f, () => {
            effectsModule.PlayEffect((int)effectsSamurai.EnergyCharging);
        });
        DOVirtual.DelayedCall(cdUltimate, () => {
            effectsModule.PlayEffect((int)effectsSamurai.UltReady);
            canUseSpecial = true; }, true);
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
            effectsModule.PlayEffect((int)effectsSamurai.PlayerHitA);
            currentLife--;
            Debug.Log("Attack step 6");
            bool isDead = currentLife <= 0;
            if (isDead)
            {
                isAlive = false;
                canReceiveDamage = false;
                character.Anim.SetBool("isAlive", isAlive);
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
        effectsModule.PlayEffect((int)effectsSamurai.endParry);
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

    public void ProjectileFlip()
    {
        if (projectil.transform.localScale.x > 0 && !character.facingRight)
        {
            Vector3 theScale = projectil.transform.localScale;
            theScale.x *= -1;
            projectil.transform.localScale = theScale;
        }
        else if (projectil.transform.localScale.x < 0 && character.facingRight)
        {
            Vector3 theScale = projectil.transform.localScale;
            theScale.x *= -1;
            projectil.transform.localScale = theScale;
        }
    }

    public enum effectsSamurai{
        startParry, endParry, jumpParticle, UltReady, PlayerHitA, PlayerHitC, EnergyCharging
    }
}
