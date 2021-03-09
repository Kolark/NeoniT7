using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SavesManager : MonoBehaviour
{
    private static SavesManager instance;
    public static SavesManager Instance { get { return instance;} }

    List<SaveWidget> saveWidgets = new List<SaveWidget>();
    List<SaveInfo> saveInfos;
    int currentSelected = 0;

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
        SaveSystem.Init();
        saveInfos = SaveSystem.LoadAll(); //Finds all SaveInfos

        for (int i = 0; i < transform.childCount; i++)
        {
            SaveWidget tosave = transform.GetChild(i).GetComponent<SaveWidget>();
            saveWidgets.Add(tosave);
            saveWidgets[i].widgetIndex = i;

        }//Finds all saveWidgets

        
        for (int i = 0; i < saveInfos.Count; i++)
        {
            Debug.Log("c : " + saveInfos.Count);
            saveWidgets[saveInfos[i].slot].SetSaveInfo(saveInfos[i]);
        }
        
    }
    private void Start()
    {
        InputController.Instance.Attack += OnDelete;
        for (int i = 0; i < saveWidgets.Count; i++)
        {
            saveWidgets[i].SetWidget();
        }
    }
    //Should Delete the current StateInfo if found
    private void OnDelete()
    {
        if (saveWidgets[currentSelected].HasSaveInfo)
        {
            saveWidgets[currentSelected].ClearSaveInfo();
            saveWidgets[currentSelected].SetWidget();
        }
    }
    
    public void setIndex(int i)
    {
        currentSelected = i;
    }
}
