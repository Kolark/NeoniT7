using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using System;


public class Chamber : MonoBehaviour
{
    BoxCollider2D col2d;
    CompositeCollider2D compositeCollider2D;
    int chamberIndex = 0;
    [SerializeField] WaveSpawn preSpawn;

    [SerializeField] List<WaveSpawn> waveSpawns;
    bool isCompleted = false;
    int wavesCounter = 0;
    List<EnemyController> enemies = new List<EnemyController>();
    public CompositeCollider2D CompositeCollider2D { get => compositeCollider2D;}
    ChamberSpawnTrigger[] spawnTriggers;

    public int wavesSize => waveSpawns.Count;


    private void Awake()
    {
        col2d = GetComponent<BoxCollider2D>();
        compositeCollider2D = GetComponent<CompositeCollider2D>();
    }

    public void chamberPreSpawn()
    {
        SpawnWave(preSpawn);
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
        SpawnWave(toSpawn);
    }

    #region SpawnMethods

    void SpawnWave(WaveSpawn wave)
    {
        for (int i = 0; i < wave.baseEnemies.Length; i++)
        {
            //Call factory to spawn EnemyA
            SpawnEnemy(wave.baseEnemies[i].transform, EnemyType.Base);
        }
        for (int i = 0; i < wave.distanceEnemies.Length; i++)
        {
            //Call factory to spawn EnemyB
            SpawnEnemy(wave.distanceEnemies[i].transform, EnemyType.Distance);
        }
        for (int i = 0; i < wave.wallEnemies.Length; i++)
        {
            //Call factory to spawn EnemyC
            SpawnEnemy(wave.wallEnemies[i].transform, EnemyType.Wall);
        }
    }

    void SpawnEnemy(Transform _transform,EnemyType type)
    {
        EnemyController enemy = EnemyFactory.Instance.CreateEnemy(_transform, type);
        enemy.AssignChamber(this);
        enemies.Add(enemy);
    }

    #endregion

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

        //----------------------TEMPORAL USAR POOL EN EL FUTURO PARA ELIMINAR PROYECTILES
        OniBProjectile[] projectiles = FindObjectsOfType<OniBProjectile>();
        for (int i = 0; i < projectiles.Length; i++)
        {
            Destroy(projectiles[i].gameObject);
        }
        //----------------------TEMPORAL USAR POOL EN EL FUTURO PARA ELIMINAR PROYECTILES


        for (int i = 0; i < enemies.Count; i++)
        {
            enemies[i].InstaDeath();
        }
        enemies = new List<EnemyController>();
        for (int i = 0; i < spawnTriggers.Length; i++)
        {
            spawnTriggers[i].TriggerReset();
        }
    }

    public void TriggerLocks()
    {
        for (int i = 0; i < spawnTriggers.Length; i++)
        {
            spawnTriggers[i].TriggerLock();
        }
    }


    #region triggers
    /// <summary>
    /// Player arrives at the chamber
    /// </summary>
    /// <param name="collision"></param>
    //private void OnTriggerEnter2D(Collider2D collision)
    //{
    //    if (collision.CompareTag("Player") && !isCompleted)
    //    {

    //        if (waveSpawns.Count == 0)
    //        {
    //            ChamberManager.Instance.UnlockNextChamber();
    //            isCompleted = true;
    //        }

    //    }
    //}


    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && ChamberManager.Instance.CanChamberTriggerExit)
        {
            float x = collision.transform.position.x;
            float y = collision.transform.position.y;
            float MaxY = transform.position.y + col2d.size.y/2f;
            float MinY = transform.position.y - col2d.size.y/2f;
            if (x > transform.position.x && y < MaxY && y > MinY)
            {
                ChamberManager.Instance.ChangeCurrentChamber(chamberIndex);
                compositeCollider2D.isTrigger = false;
                isCompleted = true;
            }
            //ChamberManager.Instance.UnlockNextChamber();//TEMPORAL
        }
    }
    #endregion


#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        col2d = GetComponent<BoxCollider2D>();
        float x = transform.position.x + col2d.size.x/2f;
        float MaxY = transform.position.y + col2d.size.y / 2f;
        float MinY = transform.position.y - col2d.size.y / 2f;
        Gizmos.DrawCube(new Vector2(x,MaxY), new Vector3(2, 2, 0));
        Gizmos.DrawCube(new Vector2(x, MinY), new Vector3(2, 2, 0));
    }
#endif

}

[Serializable]
public struct WaveSpawn
{
    public Transform[] baseEnemies;
    public Transform[] distanceEnemies;
    public Transform[] wallEnemies;
}

