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
    ChamberSpawnTrigger[] spawnTriggers;
    private void Awake()
    {
        col2d = GetComponent<Collider2D>();
        compositeCollider2D = GetComponent<CompositeCollider2D>();
        

    }

    public void setIndex(int i,Transform parentTriggers)
    {
        chamberIndex = i;
        spawnTriggers = parentTriggers.GetComponentsInChildren<ChamberSpawnTrigger>();
        for (int u = 0; u < spawnTriggers.Length; u++)
        {
            spawnTriggers[u].SetIndex(u, this);
        }
    }


    public void SpawnEnemies(int wave)
    {
        WaveSpawn toSpawn = waveSpawns[wave];
        wavesCounter++;
        for (int i = 0; i < toSpawn.baseEnemies.Length; i++)
        {
            //Call factory to spawn EnemyA
            EnemyController baseEnemy = EnemyFactory.Instance.CreateEnemy(toSpawn.baseEnemies[i].transform, EnemyType.Base);
            baseEnemy.AssignChamber(this);
            enemies.Add(baseEnemy);
        }
        for (int i = 0; i < toSpawn.distanceEnemies.Length; i++)
        {
            //Call factory to spawn EnemyB
            EnemyController distanceEnemy = EnemyFactory.Instance.CreateEnemy(toSpawn.distanceEnemies[i].transform, EnemyType.Distance);
            distanceEnemy.AssignChamber(this);
            enemies.Add(distanceEnemy);
        }
        for (int i = 0; i < toSpawn.wallEnemies.Length; i++)
        {
            //Call factory to spawn EnemyC
            EnemyController wallEnemy = EnemyFactory.Instance.CreateEnemy(toSpawn.wallEnemies[i].transform, EnemyType.Wall);
            wallEnemy.AssignChamber(this);
            enemies.Add(wallEnemy);
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

    public void ResetChamber()
    {
        wavesCounter = 0;
        for (int i = 0; i < enemies.Count; i++)
        {
            enemies[i].InstaDeath();
        }
        enemies = new List<EnemyController>();
        for (int i = 0; i < spawnTriggers.Length; i++)
        {
            spawnTriggers[i].Reset();
        }
    }


    #region triggers
    /// <summary>
    /// Player arrives at the chamber
    /// </summary>
    /// <param name="collision"></param>
    //private void OnTriggerEnter2D(Collider2D collision)
    //{
    //    if (collision.CompareTag("Player"))
    //    {
            
            

    //    }
    //}
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && ChamberManager.Instance.CanChamberTriggerExit)
        {
            ChamberManager.Instance.ChangeCurrentChamber(chamberIndex);
            compositeCollider2D.isTrigger = false;
            
            //ChamberManager.Instance.UnlockNextChamber();//TEMPORAL
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

