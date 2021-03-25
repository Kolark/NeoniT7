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
        chambers = GetComponentsInChildren<Chamber>();
        compositeCollider2Ds = GetComponentsInChildren<CompositeCollider2D>();
        for (int i = 0; i < compositeCollider2Ds.Length; i++)
        {
            compositeCollider2Ds[i].isTrigger = false;
        }
        UnlockNextChamber();
        compositeCollider2Ds[0].isTrigger = true;
        for (int i = 0; i < chambers.Length; i++)
        {
            Transform spawnTriggerParent = transform.GetChild(1).GetChild(i);
            chambers[i].setIndex(i,spawnTriggerParent);
        }

    }


    private void Start()
    {
        ChangeCurrentChamber(-1);
    }
    public void ChangeCurrentChamber(int i)
    {
        if (i == unlockedChambers)
        {
            GameManager.Instance.SetChamber(unlockedChambers);
            GameManager.Instance.Save();
            onChamberUpdate?.Invoke(unlockedChambers);
            unlockedChambers++;
        }
            Debug.Log("change i: " + i);
        if(i == (ChamberLength - 2))
        {
            SceneController.Instance.NextLevel();
        }
        CameraController.Instance.ChangeConfiner(chambers[unlockedChambers].CompositeCollider2D);
    }
    public void UnlockNextChamber()
    {
        compositeCollider2Ds[unlockedChambers+1].isTrigger = true;
    }

    public void UnlockPreviousChamber()
    {
        compositeCollider2Ds[unlockedChambers-1].isTrigger = true;
        CanChamberTriggerExit = false;
        chambers[unlockedChambers].ResetChamber();
        CameraController.Instance.ChangeConfiner(chambers[unlockedChambers-1].CompositeCollider2D);
    }

    public void EnableTriggerExit()
    {
        CanChamberTriggerExit = true;
    }
    


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
}
