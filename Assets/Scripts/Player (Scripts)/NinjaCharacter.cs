using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class NinjaCharacter : BasicCharacter
{
    //Ninja:Defensiva Dash.Se mueve hacia adelante, puede atravesar enemigos y es invulnerable durante este movimiento.
    //Ninja:Ultimate El ninja salta en su posición actual a gran altura y lanza shurikens en un cono debajo de sí mismo.
    [Header("Ninja Attributes")]
    [SerializeField] int invulnerableLayer;
    [SerializeField] GameObject projectil;
    [SerializeField] AttackInfo specialAttack;
    [SerializeField] float DashForce;
    [SerializeField] float UltimateJumpDistance;
    [SerializeField] float throwableTime;

    private LayerMask defaultLayer;
    bool isParry = false;
    public override void Defense()
    {
        if (!isAlive) return;
        if (!canUseDefense) return;
        base.Defense();
    }

    public override void StartParry()
    {
        defaultLayer = gameObject.layer;
        gameObject.layer = invulnerableLayer;
        canReceiveDamage = false;
        isParry = true;
        character.Rb.AddForce(transform.localScale.x * Vector2.right * DashForce, ForceMode2D.Impulse);
        soundModule.Play((int)NinjaSounds.Dash);
    }
    public override void EndParry()
    {
        DOVirtual.DelayedCall(1f, () => {
            gameObject.layer = defaultLayer;
            isParry = false;
            canReceiveDamage = true;
            character.Rb.velocity = Vector2.zero;
        });
    }
    public override void Throwable()
    {
        if (!isAlive) return;
        if (!canUseThrowable) return;
        if (!character.Grounded) return;
        base.Throwable();
        DOVirtual.DelayedCall(throwableTime, () => {
            GameObject gameObject = Instantiate(projectil, firstAttack.pos.position, Quaternion.identity);
            Proyectil proyectil = gameObject.GetComponent<Proyectil>();
            proyectil.push(Vector2.right * transform.localScale.x);
        });
    }
    public override void Ultimate()///Dotween con ticks
    {
        if (!isAlive) return;
        if (!canUseSpecial) return;
        if (!character.Grounded) return;
        base.Ultimate();


        onUltAbility?.Invoke(cdUltimate);

        DOVirtual.DelayedCall(cdUltimate, () => { canUseSpecial = true; }, true);
        Character.CanJump = false;
        //salte, se quede arriba, y luego caiga

        DOTween.Sequence().Append(transform.DOLocalMoveY(UltimateJumpDistance,0.5f)).AppendCallback(()=> {

            Vector2 pos = transform.position;
            Collider2D[] Hit = Physics2D.OverlapCircleAll(specialAttack.pos.position, specialAttack.radius, specialAttack.layer);
            for (int i = 0; i < Hit.Length; i++)
            {
                IEnemyHurtBox enemy = Hit[i]?.GetComponent<IEnemyHurtBox>();
                if (enemy != null)
                {
                    for (int j = 0; j < 6; j++)
                    {
                        enemy.OnReceiveDamage();
                        ScoreManager.Instance?.AddScore(enemy.getPos().position, 200);
                    }
                }
            }

            DOVirtual.DelayedCall(.3f, null, true).OnUpdate(() =>
            {
                transform.position = pos;
            });
            //Se mantiene en el aire
        });
    }

    public enum NinjaSounds
    {
        combo1, combo2, combo3, getHit, Jump, Death, Ultimate, UltimateCharged, Dash
    }

}
