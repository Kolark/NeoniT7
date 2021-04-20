using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.UIElements;
public class PrefabTools : EditorWindow
{

    [MenuItem("PrefabTools/Open")]
    public static void ShowWindow()
    {
        // Opens the window, otherwise focuses it if it’s already open.
        var window = GetWindow<PrefabTools>();

        // Adds a title to the window.
        window.titleContent = new GUIContent("PrefabTools");

        // Sets a minimum size to the window.
        window.minSize = new Vector2(250, 50);
    }


    private void OnEnable()
    {
        #region initialization
        var root = rootVisualElement;
        root.styleSheets.Add(Resources.Load<StyleSheet>("PrefabTools_Style"));
        var quickToolVisualTree = Resources.Load<VisualTreeAsset>("PrefabTools");
        quickToolVisualTree.CloneTree(root);
    #endregion
        var toolButtons = root.Query<Button>(className: "createPrefab"); //Create Prefabs
        toolButtons.ForEach(SetupButton);

        var boxes = root.Query<Box>(className:"managerState");
        //boxes.ForEach(CheckStates);

        var refreshButton = root.Q<Button>(className: "RefreshButton");
        refreshButton.clickable.clicked += () => { boxes.ForEach(CheckStates);};
    }
    private void CheckStates(Box box)
    {
        MonoBehaviour @gameobject;
        switch (box.name)
        {
            case "AudioManager":
                @gameobject = FindObjectOfType<AudioManager>();
                break;
            case "GameManager":
                @gameobject = FindObjectOfType<GameManager>();
                break;
            case "SceneController":
                @gameobject = FindObjectOfType<SceneController>();
                break;
            case "InputController":
                @gameobject = FindObjectOfType<InputController>();
                break;
            case "CameraController":
                @gameobject = FindObjectOfType<CameraController>();
                break;
            case "ChamberManager":
                @gameobject = FindObjectOfType<ChamberManager>();
                break;
            default:
                @gameobject = null;
                break;
        }
        if (@gameobject != null)
        {
            box.AddToClassList("green_state");
            box.RemoveFromClassList("red_state");
        }
        else
        {
            box.AddToClassList("red_state");
            box.RemoveFromClassList("green_state");
        }
    }

    private void CheckManagers()
    {

    }

    private void SetupButton(Button button)
    {
        
        button.clickable.clicked += () => 
        {
            CreatePrefab(button.text);
            var boxes = rootVisualElement.Query<Box>(className: "managerState");
            boxes.ForEach(CheckStates);
        };
    }

    private void CreatePrefab(string obj)
    {
        Object prefab = AssetDatabase.LoadAssetAtPath($"Assets/Prefabs/$h{obj}.prefab", typeof(GameObject));
        
        //GameObject toInstatiate = Instantiate(prefab, Vector3.zero, Quaternion.identity) as GameObject;
        GameObject toInstatiate = PrefabUtility.InstantiatePrefab(prefab) as GameObject;
        switch (obj)
        {
            case "SceneController":
                PrefabUtility.UnpackPrefabInstance(toInstatiate, PrefabUnpackMode.Completely, InteractionMode.UserAction);
                break;
            case "CameraController":
                PrefabUtility.UnpackPrefabInstance(toInstatiate, PrefabUnpackMode.Completely, InteractionMode.UserAction);
                break;
            case "ChamberManager":
                PrefabUtility.UnpackPrefabInstance(toInstatiate, PrefabUnpackMode.Completely, InteractionMode.UserAction);
                break;
        }
        
    }


    private void DebugButton(string todebug)
    {
        Debug.Log(todebug);
    }
}

public enum PrefabTypes
{
    AudioManager,
    GameManager,
    SceneController
}