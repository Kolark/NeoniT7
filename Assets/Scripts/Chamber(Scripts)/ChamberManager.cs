using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class ChamberManager : MonoBehaviour
{

    private static ChamberManager instance;
    public static ChamberManager Instance { get => instance; }

    [SerializeField] float test;

    int unlockedChambers = 0; // 0 - n
    public int UnlockedChambers { get => unlockedChambers;}
    Chamber[] chambers;
    CompositeCollider2D[] compositeCollider2Ds;

    public Action<int> onChamberUpdate;

    //Represents whether or not the player is able to go to the next Chamber. 
    //When he just wiped a chamber or the checkpoint got him there.
    public bool CanChamberTriggerExit = true;

    public int ChamberLength { get => chambers.Length;}

    private void Awake()
    {
        #region singleton
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }
        #endregion

        if(GameManager.Instance != null)
        {
            unlockedChambers = GameManager.Instance.Current.chamber;
        }

        chambers = GetComponentsInChildren<Chamber>();
        compositeCollider2Ds = GetComponentsInChildren<CompositeCollider2D>();

        for (int i = 0; i < compositeCollider2Ds.Length; i++)
        {
            compositeCollider2Ds[i].isTrigger = false;
        }

        UnlockNextChamber();
        compositeCollider2Ds[unlockedChambers].isTrigger = true;
        for (int i = 0; i < chambers.Length; i++)
        {
            Transform spawnTriggerParent = transform.GetChild(1).GetChild(i);
            chambers[i].setIndex(i,spawnTriggerParent);
        }
        chambers[unlockedChambers].TriggerLocks();
        
    }


    private void Start()
    {
        ChangeCurrentChamber(-1);
        if (unlockedChambers != 0) { unlockedChambers++; }
    }
    //Called when a chamber was cleared and ontriggerExit was detected
    public void ChangeCurrentChamber(int i)
    {
        if (i == unlockedChambers)
        {
            GameManager.Instance.SetChamber(unlockedChambers);
            GameManager.Instance.Save();
            onChamberUpdate?.Invoke(unlockedChambers);
            unlockedChambers++;
            if(chambers[unlockedChambers].wavesSize == 0 && i != (ChamberLength - 2))
            {
                UnlockNextChamber();
            }
        }
        if(i == (ChamberLength - 2))
        {
            SceneController.Instance.NextLevel();
        }
        CameraController.Instance.ChangeConfiner(chambers[unlockedChambers].CompositeCollider2D);
    }

    public void UnlockNextChamber()
    {
        compositeCollider2Ds[unlockedChambers+1].isTrigger = true;
        chambers[unlockedChambers+1].chamberPreSpawn();
    }

    public void UnlockPreviousChamber()
    {
        compositeCollider2Ds[unlockedChambers-1].isTrigger = true;//Makes the last played chamber trigger.
        CanChamberTriggerExit = false;//False so that when a triggerexit is detected, it doesn't unlock a chamber
        chambers[unlockedChambers].ResetChamber();//Reset the chamber in which the player died.
        chambers[unlockedChambers].chamberPreSpawn();
        CameraController.Instance.ChangeConfiner(chambers[unlockedChambers-1].CompositeCollider2D);//Changescameraconfiner
    }
    //Called with delay, it enables to go to the next Chamber
    public void EnableTriggerExit()
    {
        CanChamberTriggerExit = true;
    }


    #region Gizmos
    Color[] gizmosColors = { Color.red, Color.blue, Color.green, Color.cyan, Color.yellow, Color.magenta };
    private void OnDrawGizmos()
    {
        BoxCollider2D[] colliders = GetComponentsInChildren<BoxCollider2D>();
        for (int i = 0; i < colliders.Length; i++)
        {
            Gizmos.color = gizmosColors[i % gizmosColors.Length] - new Color(0, 0, 0, 0.5f);
            Gizmos.DrawCube(colliders[i].transform.position,
                new Vector3(colliders[i].size.x, colliders[i].size.y, 0));
        }
    }
    #endregion

    #region onValidate
    private void OnValidate()
    {
        //BoxCollider2D[] colliders = GetComponentsInChildren<BoxCollider2D>();
        BoxCollider2D[] colliders = new BoxCollider2D[transform.childCount];
        for (int i = 0; i < transform.childCount; i++)
        {
            colliders[i] = transform.GetChild(0).GetChild(i).GetComponent<BoxCollider2D>();
        }
        float Xpos = transform.position.x;
        float Ypos = transform.position.y;
        colliders[0].transform.position = new Vector2(Xpos, Ypos);
        for (int i = 1; i < colliders.Length; i++)
        {
            Xpos += (colliders[i].size.x) / 2 + (colliders[i - 1].size.x) / 2;
            colliders[i].transform.position = new Vector3(Xpos, Ypos, 0);
            
        }
    }
    #endregion
}
