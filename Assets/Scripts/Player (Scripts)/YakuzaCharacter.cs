using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class YakuzaCharacter : BasicCharacter
{
    bool canTankDamage = false;
    [SerializeField] GameObject projectile;
    [SerializeField] AttackInfo specialAttack;
    [SerializeField] AttackInfo explosiveinfo;
    [SerializeField] float UltimateWalkDistance;
    [SerializeField] Vector2 dir;
    [SerializeField] float walkUltiTime, jumpUltiTime, suspensionUltiTime, fallUltiTime;
    [SerializeField] float throwableTime, shieldTime;


    [Header("Ulti Attributes")]
    [SerializeField] float MagnitudJump;
    [SerializeField] float MagnitudFall;


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
        effectsModule.PlayEffect((int)effectsYakuza.Shield);
    }
    public override void EndParry()
    {
        Invoke("EndShield", shieldTime);
    }

    void EndShield()
    {
        canTankDamage = false;
        canReceiveDamage = true;
        effectsModule.StopEffect((int)effectsYakuza.Shield);
    }

    public override void Throwable()
    {
        if (!isAlive) return;
        if (!canUseThrowable) return;
        if (!character.Grounded) return;
        base.Throwable();
        DOVirtual.DelayedCall(throwableTime, null, true).OnComplete(() =>
        {
            GameObject gameObject = Instantiate(projectile, firstAttack.pos.position, Quaternion.identity);
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
        onUltAbility?.Invoke(cdUltimate);
        DOVirtual.DelayedCall(cdUltimate, () => { effectsModule.PlayEffect((int)effectsYakuza.UltReady); 
            canUseSpecial = true; }, true);
        effectsModule.PlayEffect((int)effectsYakuza.Ulti);
        effectsModule.StopEffect((int)effectsYakuza.UltReady);
        Vector2 vec = new Vector2(dir.x * transform.localScale.x, dir.y);
        Vector2 vec2 = new Vector2(dir.x * transform.localScale.x, -dir.y);

        ///Aca va la ultimate
        ///
        DOTween.Sequence()
            .Append(DOVirtual.DelayedCall(0.5f, null).OnUpdate(() => 
            {
                character.Rb.velocity = (Vector2.right*transform.localScale.x + Vector2.up).normalized * MagnitudJump;
            }))
            .Append(DOVirtual.DelayedCall(0.4f,null).OnUpdate(()=> 
            {
                character.Rb.velocity = (Vector2.right * transform.localScale.x + Vector2.down).normalized * MagnitudFall;
            }))
            .AppendCallback(() =>
            {
                character.Rb.velocity = Vector2.zero;
                Collider2D[] Hit = Physics2D.OverlapCircleAll(specialAttack.pos.position, specialAttack.radius, specialAttack.layer);
                for (int i = 0; i < Hit.Length; i++)
                {
                    IEnemyHurtBox enemy = Hit[i]?.GetComponent<IEnemyHurtBox>();
                    if (enemy != null)
                    {
                        for (int y = 0; y < 3; y++)
                        {
                            enemy.OnReceiveDamage();
                            ScoreManager.Instance?.AddScore(enemy.getPos().position, 150);
                        }
                        
                    }
                }
            })
            ;
    }
    public override void Damage()
    {
        Debug.Log("Attack step 5");
        if (canReceiveDamage)
        {
            currentLife--;
            Debug.Log("Attack step 6");
            effectsModule.PlayEffect((int)effectsYakuza.PlayerHitA);
            bool isDead = currentLife <= 0;
            if (isDead)
            {
                Death();
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

    public override void Death()
    {
        isAlive = false;
        canReceiveDamage = false;
        character.Anim.SetTrigger("Death");
        DOVirtual.DelayedCall(0.8f, () => { SceneController.Instance.GoToLastCheckpoint(); });
    }

    public enum effectsYakuza
    {
        Shield, Ulti, jumpParticle, UltReady, PlayerHitA, PlayerHitC
    }
}
