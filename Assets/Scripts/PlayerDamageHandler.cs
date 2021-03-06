﻿using System.Collections;
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
    }

    public void Death()
    {
        player.Death();
    }

    public Transform getPos()
    {
        return transform;
    }
}
