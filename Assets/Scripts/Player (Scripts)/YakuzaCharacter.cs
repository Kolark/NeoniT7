using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class YakuzaCharacter : BasicCharacter
{
    bool canTankDamage = false;
    [SerializeField] GameObject projectil;
    [SerializeField] AttackInfo specialAttack;
    [SerializeField] AttackInfo explosiveinfo;
    [SerializeField] float UltimateWalkDistance;
    [SerializeField] Vector2 dir;
    [SerializeField] float walkUltiTime, jumpUltiTime, suspensionUltiTime, fallUltiTime;
    [SerializeField] float throwableTime;

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
        DOVirtual.DelayedCall(throwableTime, null, true).OnComplete(() =>
        {
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
        DOVirtual.DelayedCall(cdUltimate, () => { canUseSpecial = true; }, true);
        Vector2 vec = new Vector2(dir.x * transform.localScale.x, dir.y);
        Vector2 vec2 = new Vector2(dir.x * transform.localScale.x, -dir.y);

        ///Aca va la ultimate
        DOTween.Sequence()
            .Append(transform.DOLocalMoveX(transform.position.x + (UltimateWalkDistance * transform.localScale.x), walkUltiTime).SetEase(Ease.InOutSine))
            .Append(transform.DOLocalMove((Vector2)transform.position + vec, jumpUltiTime).SetEase(Ease.OutSine))
            .AppendCallback(() => 
            {

            Vector2 pos = transform.position;

            DOVirtual.DelayedCall(suspensionUltiTime, null, true).OnUpdate(() =>
            {
                transform.position = pos;

            }).OnComplete(() => 
            {
                
                transform.DOLocalMove((Vector2)transform.position + vec2, fallUltiTime).SetEase(Ease.OutSine).OnComplete(() =>
                {
                Collider2D[] Hit = Physics2D.OverlapCircleAll(specialAttack.pos.position, specialAttack.radius, specialAttack.layer);
                for (int i = 0; i < Hit.Length; i++)
                {
                    IEnemyHurtBox enemy = Hit[i]?.GetComponent<IEnemyHurtBox>();
                    if (enemy != null)
                    {
                        enemy.OnReceiveDamage();
                        ScoreManager.Instance?.AddScore(enemy.getPos().position, 150);
                    }
                    }
                });
            });
            //Se mantiene en el aire
            });
    }
    public override void Damage()
    {
        Debug.Log("Attack step 5");
        if (canReceiveDamage)
        {
            currentLife--;
            Debug.Log("Attack step 6");
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

}
