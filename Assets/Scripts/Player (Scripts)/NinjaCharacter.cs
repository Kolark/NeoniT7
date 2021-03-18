using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class NinjaCharacter : BasicCharacter
{
    //Ninja:Defensiva Dash.Se mueve hacia adelante, puede atravesar enemigos y es invulnerable durante este movimiento.
    //Ninja:Ultimate El ninja salta en su posición actual a gran altura y lanza shurikens en un cono debajo de sí mismo.
    
    [SerializeField] int invulnerableLayer;
    private LayerMask defaultLayer;
    bool isParry = false;
    [SerializeField] GameObject projectil;
    [SerializeField] AttackInfo specialAttack;
    [SerializeField] float DashForce;
    [SerializeField] float UltimateJumpDistance;
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
    }
    public override void EndParry()
    {
        gameObject.layer = defaultLayer;
        isParry = false;
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
    public override void Ultimate()///Dotween con ticks
    {
        if (!isAlive) return;
        if (!canUseSpecial) return;
        if (!character.Grounded) return;
        base.Ultimate();
        DOVirtual.DelayedCall(cdUltimate, () => { canUseSpecial = true; }, true);
        Character.CanJump = false;
        //salte, se quede arriba, y luego caiga

        DOTween.Sequence().Append(transform.DOLocalMoveY(UltimateJumpDistance,1.0f)).AppendCallback(()=> {
            Vector2 pos = transform.position;

            DOVirtual.DelayedCall(4, null, true).OnUpdate(() =>
            {
                transform.position = pos;

            });
            //Se mantiene en el aire
        });
    }
   

}
