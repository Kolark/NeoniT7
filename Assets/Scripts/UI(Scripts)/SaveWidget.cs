using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Localization.Components;
using UnityEngine.Localization;

public class SaveWidget : UIWidget
{

    /// <summary>
    /// SaveInfo Class Information
    /// </summary>
    public SaveInfo saveInfo;
    bool hasSaveInfo = false;
    public bool HasSaveInfo { get => hasSaveInfo;}
    /// <summary>
    /// Index corresponding to the SaveWidget gameobject
    /// </summary>
    public int widgetIndex = 0;
    //public string lastSaved { get { return saveInfo.lastSaved.ToShortDateString(); } }
    //public string lastlevelplayed { get { return saveInfo.currentScene.ToString(); } }

    private RectTransform emptyUI;
    private RectTransform saveInfoUI;

    protected override void Awake()
    {
        base.Awake();
        emptyUI = transform.GetChild(0).GetComponent<RectTransform>();
        saveInfoUI= transform.GetChild(1).GetComponent<RectTransform>();
    }
    public string lastSaved { get {return "date"; } }
    public string lastlevelplayed { get { return "scene"; } }


    protected override void OnSelect()
    {
        base.OnSelect();
        SavesManager.Instance?.setIndex(widgetIndex);
    }

    
    protected override void OnDeselect()
    {
        base.OnDeselect();
        
    }

    /// <summary>
    /// Creates or uses a saveInfo Struct
    /// </summary>
    protected override void OnPointerDown()
    {
        base.OnPointerDown();
        AudioManager.Instance.Play("Nota-001");
        if (hasSaveInfo)
        {
            GameManager.Instance.ChangeScene(saveInfo);
        }
        else
        {
            GameManager.Instance.ChangeScene(new SaveInfo(GameScene.testfelipe,0,widgetIndex,DateTime.Now));
        }
    }


    /// <summary>
    /// Will set all of the elements inside the widget according to the SaveInfoClass
    /// </summary>

    public void SetSaveInfo(SaveInfo saveInfo)
    {
        this.saveInfo = saveInfo;
        hasSaveInfo = true;
    }
    public void ClearSaveInfo()
    {
        hasSaveInfo = false;
        
    }
    public void SetWidget ()
    {
        saveInfoUI.gameObject.SetActive(hasSaveInfo);
        emptyUI.gameObject.SetActive(!hasSaveInfo);
    }
    


    //States-Empty-Loaded
    //TO-DO
    //Let Emptys Create a new Load State


    //TO-DO Connect SaveManager with the SaveWidgets, 
    //Add the option to create and delete savings(create is automatic)
    //Add Localization to the string in the Save Method
    //Do everything else detailed onthe sayvesystem. SceneController,GameManager.
}
