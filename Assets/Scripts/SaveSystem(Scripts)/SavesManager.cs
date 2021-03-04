using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SavesManager : MonoBehaviour
{
    private static SavesManager instance;
    public static SavesManager Instance => instance;

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
        
        saveInfos = SaveSystem.LoadAll(); //Finds all SaveInfos

        for (int i = 0; i < transform.childCount; i++)
        {
            SaveWidget tosave = transform.GetChild(i).GetComponent<SaveWidget>();
            saveWidgets[i].widgetIndex = i;

            saveWidgets.Add(tosave);
        }//Finds all saveWidgets
        for (int i = 0; i < saveInfos.Count; i++)
        {
            saveWidgets[saveInfos[i].slot].saveInfo = saveInfos[i];
        }
    }
    private void Start()
    {
        InputController.Instance.Attack += OnDelete;
    }

    private void OnDelete()
    {

    }
    public void setIndex(int i)
    {
        currentSelected = i;
    }
}
