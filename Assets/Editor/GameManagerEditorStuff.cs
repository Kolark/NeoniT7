using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
[CustomEditor(typeof(GameManager))]
public class GameManagerEditorStuff : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        GameManager gameManager = (GameManager)target;

        if (GUILayout.Button("WRITE SLOT"))
        {
            SaveSystem.Save(gameManager.SaveInfoEDITOR);
        }
    }
}
