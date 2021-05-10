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
            if (collision.CompareTag("Player"))
            {
                hasActivated = true;
                chamber.SpawnEnemies(index);

            }
        }
    }

    public void TriggerReset()
    {
        hasActivated = false;        
    }

    public void TriggerLock()
    {
        hasActivated = true;        
    }

    private void OnDrawGizmos()
    {
        BoxCollider2D boxCollider2D = GetComponent<BoxCollider2D>();

        Gizmos.color = new Color(1,1,1,0.5f);
        Gizmos.DrawCube(transform.position,
        new Vector3(boxCollider2D.size.x, boxCollider2D.size.y, 0));
    }

}
