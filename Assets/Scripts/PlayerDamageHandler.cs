using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDamageHandler : MonoBehaviour, IEnemyHurtBox
{
    BasicCharacter player;
    void Awake() {
        player = GetComponentInParent<BasicCharacter>();
        
    }
    public void OnReceiveDamage() {
        player.Damage();
        Debug.Log("Attack step 4");
    }

    public void Death()
    {
        player.Death();
    }

}
