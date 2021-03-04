using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Localization.Components;
public class SaveWidget : UIWidget
{
    public SaveInfo saveInfo;
    public int widgetIndex;
    //public string lastSaved { get { return saveInfo.lastSaved.ToShortDateString(); } }
    //public string lastlevelplayed { get { return saveInfo.currentScene.ToString(); } }
    protected override void Awake()
    {
        base.Awake();
        Debug.Log("awake");
    }
    public string lastSaved { get {Debug.Log("Get"); return "date"; } }
    public string lastlevelplayed { get { return "scene"; } }

    

    protected override void OnSelect()
    {
        base.OnSelect();
        SavesManager.Instance.setIndex(widgetIndex);
    }
    protected override void OnDeselect()
    {
        base.OnDeselect();
    }
    public void SetWidget ()
    {

    }
    //TO-DO Connect SaveManager with the SaveWidgets, 
    //Add the option to create and delete savings(create is automatic)
    //Add Localization to the string in the Save Method
    //Do everything else detailed onthe sayvesystem. SceneController,GameManager.
}
