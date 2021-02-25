﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDamageHandler : MonoBehaviour, IEnemyHurtBox
{
    EnemyController enemy;
    void Awake() {
        enemy = GetComponentInParent<EnemyController>();
    }

    public void OnReceiveDamage() {
        Destroy(enemy.gameObject);
    }
}
