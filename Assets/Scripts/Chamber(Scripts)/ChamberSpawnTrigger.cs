using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChamberSpawnTrigger : MonoBehaviour
{
    bool hasActivated = false;
    int index = 0;
    Chamber chamber;
    public void SetIndex(int i,Chamber parentChamber)
    {
        index = i;
        chamber = parentChamber;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!hasActivated)
        {
            hasActivated = true;
            chamber.SpawnEnemies(index);;
        }
    }
}
