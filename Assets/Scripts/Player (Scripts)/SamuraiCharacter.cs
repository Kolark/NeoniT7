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
    [Header("Ulti")]
    [SerializeField] GameObject projectilUlti;
    [SerializeField] Transform UltiTransform;
    [SerializeField] AttackInfo specialAttack;
    [SerializeField] float delayProjectil;
    [SerializeField] float ultimateOffsetTime;
    [Space]
    [Space]
    [SerializeField] float projectileDelay =.45f;
    public override void Defense()
    {
        if (!isActive) return;
        if (!isAlive) return;
        if (GameManager.Instance.IsPaused) return;
        if (!canUseDefense) return;
        base.Defense();
        
    }

    public override void StartParry()
    {
        canReceiveDamage = false;
        isParry = true;
        effectsModule.PlayEffect((int)effectsSamurai.startParry);
    }
    public override void EndParry()
    {
        isParry = false;
        canReceiveDamage = true;
    }
    public override void Throwable()
    {
        if (!isActive) return;
        if (!isAlive) return;
        if (!canUseThrowable) return;
        if (!character.Grounded) return;
        if (GameManager.Instance.IsPaused) return;
        base.Throwable();
        DOVirtual.DelayedCall(projectileDelay, null, true).OnComplete(() =>
        {

            GameObject gameObject = Instantiate(projectil, firstAttack.pos.position, Quaternion.identity);
            Proyectil proyectil = gameObject.GetComponent<Proyectil>();
            proyectil.push(Vector2.right * transform.localScale.x);
        });
    }
    public override void Ultimate()
    {
        if (!isActive) return;
        if (!isAlive) return;
        if (!canUseSpecial) return;
        if (!character.Grounded) return;
        if (GameManager.Instance.IsPaused) return;
        base.Ultimate();
        effectsModule.StopEffect((int)effectsSamurai.UltReady);
        DOVirtual.DelayedCall(0.3f, () => {
            effectsModule.PlayEffect((int)effectsSamurai.EnergyCharging);
        });
        DOVirtual.DelayedCall(delayProjectil, () => {
            GameObject gameObject = Instantiate(projectilUlti, UltiTransform.position, Quaternion.identity);
            soundModule.Play((int)SamuraiSounds.Ultimate);
            Proyectil proyectil = gameObject.GetComponent<Proyectil>();
            proyectil.push(Vector2.right * transform.localScale.x);

        });

        onUltAbility?.Invoke(cdUltimate);
        DOVirtual.DelayedCall(cdUltimate, () => {
            soundModule.Play((int)SamuraiSounds.UltimateCharged);
            effectsModule.PlayEffect((int)effectsSamurai.UltReady);
            canUseSpecial = true; }, true);
        ultimateAnim.SetTrigger("Ultimate");

        DOVirtual.DelayedCall(ultimateOffsetTime,()=> {
            Collider2D[] Hit = Physics2D.OverlapCircleAll(specialAttack.pos.position, specialAttack.radius, specialAttack.layer);
            for (int i = 0; i < Hit.Length; i++)
            {
                IEnemyHurtBox enemy = Hit[i]?.GetComponent<IEnemyHurtBox>();
                if (enemy != null)
                {
                    enemy.OnReceiveDamage();
                    ScoreManager.Instance?.AddScore(enemy.getPos().position, 200);
                }
            }
        });

    }

    public override void Damage()
    {
        if (canReceiveDamage)
        {
            effectsModule.PlayEffect((int)effectsSamurai.PlayerHitA);
            currentLife--;
            onLifeChange?.Invoke(currentLife);
            soundModule.Play((int)CharacterSounds.getHit);
            bool isDead = currentLife <= 0;
            if (isDead)
            {
                Death();
                return;
            }
            character.Anim.SetTrigger("Damage");
        }
        else if(isParry)
        {
            //
            Counter();
            isParry = false;
            canReceiveDamage = true;
        }
    }

    public override void Death()
    {
        isAlive = false;
        OnCharacterDeath?.Invoke();
        soundModule.Play((int)SamuraiSounds.Death);
        canReceiveDamage = false;
        character.Anim.SetTrigger("Death");
        DOVirtual.DelayedCall(0.8f,()=> { SceneController.Instance.GoToLastCheckpoint(); });
        
    }

    public override void Counter()
    {
        effectsModule.PlayEffect((int)effectsSamurai.endParry);
        soundModule.Play((int)SamuraiSounds.Parry);
        character.Anim.SetTrigger("Counter");
        Collider2D[] Hit = Physics2D.OverlapCircleAll(counter.pos.position, counter.radius, counter.layer);
        for (int i = 0; i < Hit.Length; i++)
        {
            IEnemyHurtBox enemy = Hit[i]?.GetComponent<IEnemyHurtBox>();
            if (enemy != null)
            {
                enemy.OnReceiveDamage();
                ScoreManager.Instance?.AddScore(enemy.getPos().position, 1000);
            }
        }
    }
    protected override void OnDrawGizmos()
    {
        base.OnDrawGizmos();
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(specialAttack.pos.position, specialAttack.radius);

    }

    public enum effectsSamurai{
        startParry, endParry, jumpParticle, UltReady, PlayerHitA, PlayerHitC, EnergyCharging
    }
    public enum SamuraiSounds
    {
        combo1, combo2, combo3, getHit, Jump, Death, Ultimate, UltimateCharged,Parry
    }
}
