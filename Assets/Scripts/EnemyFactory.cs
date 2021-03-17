using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFactory : MonoBehaviour
{
    private static EnemyFactory instance;
    public static EnemyFactory Instance { get => instance;}



    [SerializeField] GameObject[] enemies;



    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    public EnemyController CreateEnemy(Transform pos,EnemyTipe type)
    {
        GameObject instance = Instantiate(enemies[(int)type], pos.position, Quaternion.identity);
        EnemyController enemyController = instance.GetComponent<EnemyController>();
        return enemyController;
        
    }

}
public enum EnemyTipe { Base, Distance, Wall }//Temporal