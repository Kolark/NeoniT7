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

    Sequence ultiSequence;

    public override void Defense()
    {
        if (!isActive) return;
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
        effectsModule.PlayEffect((int)effectsNinja.Dash);
        soundModule.Play((int)NinjaSounds.Dash);
    }
    public override void EndParry()
    {
        DOVirtual.DelayedCall(0.15f, () => {
            gameObject.layer = defaultLayer;
            isParry = false;
            canReceiveDamage = true;
            character.Rb.velocity = Vector2.zero;
            Debug.Log("FORCED DOWN TO ZERO");
        });
    }
    public override void Throwable()
    {
        if (!isActive) return;
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
        if (!isActive) return;
        if (!isAlive) return;
        if (!canUseSpecial) return;
        if (!character.Grounded) return;
        base.Ultimate();

        //UltimateCoolDown
        onUltAbility?.Invoke(cdUltimate);
        DOVirtual.DelayedCall(cdUltimate, () => {
            effectsModule.PlayEffect((int)effectsNinja.UltReady);
            canUseSpecial = true; }, true);

        Character.CanJump = false;
        //salte, se quede arriba, y luego caiga

            //.Append(transform.DOLocalMoveY(UltimateJumpDistance,0.5f))

        ultiSequence = DOTween.Sequence()
            
            .Append(DOVirtual.DelayedCall(0.5f,null).OnUpdate(()=> 
            {
                character.Rb.velocity = Vector2.up * UltimateJumpDistance;
            }))
            .AppendCallback(()=> {
            character.Rb.velocity = Vector2.zero;
            Vector2 pos = transform.position;
            effectsModule.PlayEffect((int)effectsNinja.UltiRange);
            effectsModule.StopEffect((int)effectsNinja.UltReady);
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
                character.Rb.velocity = Vector2.zero;
                transform.position = pos;
            });
            //Se mantiene en el aire
        });
    }

    public override void Damage()
    {
        float f = currentLife;
        base.Damage();
        if(currentLife!=f) effectsModule.PlayEffect((int)effectsNinja.PlayerHitA);

    }
    //private void OnCollisionEnter2D(Collision2D collision)
    //{
    //    ultiSequence?.Kill();
    //}
    public enum effectsNinja
    {
        Dash, UltiRange, jumpParticle, UltReady, PlayerHitA, PlayerHitC
    }

    public enum NinjaSounds
    {
        combo1, combo2, combo3, getHit, Jump, Death, Ultimate, UltimateCharged, Dash
    }

}
