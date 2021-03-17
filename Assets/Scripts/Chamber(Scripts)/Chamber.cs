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
    int wavesCounter = 0;
    List<EnemyController> enemies = new List<EnemyController>();
    public CompositeCollider2D CompositeCollider2D { get => compositeCollider2D;}

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
        wavesCounter++;
        for (int i = 0; i < toSpawn.baseEnemies.Length; i++)
        {
            //Call factory to spawn EnemyA
            enemies
                .Add(
                EnemyFactory.Instance.CreateEnemy(toSpawn.baseEnemies[i].transform, EnemyTipe.Base));
            
        }
        for (int i = 0; i < toSpawn.distanceEnemies.Length; i++)
        {
            //Call factory to spawn EnemyB
            enemies
                .Add(
                EnemyFactory.Instance.CreateEnemy(toSpawn.baseEnemies[i].transform, EnemyTipe.Distance));
        }
        for (int i = 0; i < toSpawn.wallEnemies.Length; i++)
        {
            //Call factory to spawn EnemyC
            enemies
                .Add(
                EnemyFactory.Instance.CreateEnemy(toSpawn.baseEnemies[i].transform, EnemyTipe.Wall));
        }
    }


    public void OnEnemyDead(EnemyController enemyController)
    {
        enemies.Remove(enemyController);
        if(enemies.Count == 0 && wavesCounter == waveSpawns.Count)
        {
            ChamberManager.Instance.UnlockNextChamber();
        }
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
            ChamberManager.Instance.UnlockNextChamber();//TEMPORAL
            

        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            ChamberManager.Instance.ChangeCurrentChamber(chamberIndex);
            compositeCollider2D.isTrigger = false;

        }
    }
    #endregion

    private void OnDrawGizmos()
    {
        
    }

}

[Serializable]
public struct WaveSpawn
{
    public Transform[] baseEnemies;
    public Transform[] distanceEnemies;
    public Transform[] wallEnemies;
}

