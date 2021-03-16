using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using System;


public class Chamber : MonoBehaviour
{
    Collider2D col2d;
    CompositeCollider2D compositeCollider2D;
    int chamberIndex = 0;

    [SerializeField] List<WaveSpawn> waveSpawns;

    bool hasUnlockedNextRoom = false;



    private void Awake()
    {
        col2d = GetComponent<Collider2D>();
        compositeCollider2D = GetComponent<CompositeCollider2D>();
        ChamberSpawnTrigger[] spawnTriggers = GetComponentsInChildren<ChamberSpawnTrigger>();
        for (int i = 0; i < spawnTriggers.Length; i++)
        {
            spawnTriggers[i].SetIndex(i, this);
        }

    }

    public void setIndex(int i)
    {
        chamberIndex = i;
    }


    public void SpawnEnemies(int wave)
    {
        WaveSpawn toSpawn = waveSpawns[wave];

        for (int i = 0; i < toSpawn.baseEnemies.Length; i++)
        {
            //Call factory to spawn EnemyA
        }
        for (int i = 0; i < toSpawn.distanceEnemies.Length; i++)
        {
            //Call factory to spawn EnemyB
        }
        for (int i = 0; i < toSpawn.wallEnemies.Length; i++)
        {
            //Call factory to spawn EnemyC
        }
    }


    public void OnEnemyDead()
    {

    }

    #region triggers
    /// <summary>
    /// Player arrives at the chamber
    /// </summary>
    /// <param name="collision"></param>
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            
        }
    }
    #endregion
    
}

[Serializable]
public struct WaveSpawn
{
    public Transform[] baseEnemies;
    public Transform[] distanceEnemies;
    public Transform[] wallEnemies;
}

