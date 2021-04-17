using UnityEngine;
public interface IEnemyHurtBox
{
    void OnReceiveDamage();

    Transform getPos();
}