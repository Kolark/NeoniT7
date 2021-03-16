using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChamberManager : MonoBehaviour
{

    private static ChamberManager instance;
    public static ChamberManager Instance { get => instance; }

    int unlockedChambers = 0;
    public int UnlockedChambers { get => unlockedChambers;}
    Chamber[] chambers;
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
        for (int i = 0; i < chambers.Length; i++)
        {

        }

    }

    public void UnlockChamber(int i)
    {
        unlockedChambers = i;
    }

}
