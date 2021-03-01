using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;
public class GameManager : MonoBehaviour
{
    private static GameManager instance;
    public static GameManager Instance => instance;

    private GameScene currentScene;
    public GameScene CurrentScene { get => currentScene;}

    private SceneType currentSceneType;
    private ControllerType controllerType {get=> InputController.Instance.CurrentControlScheme;}

    private int saveSlot;
    public int Saveslot { 
        get => saveSlot; set
        {
            if(value >=0 && value < SaveSystem.SavesNumber)
            {
                saveSlot = value;
            }
            else
            {
                saveSlot = SaveSystem.SavesNumber-1;
            }
        }
    }
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            currentScene = GameScene.MainScreen;
        }
        else
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);
    }
    /// <summary>
    /// Receives a SceneBuild type of Parameter and loads that current scene
    /// </summary>
    /// <param name="scene"></param>
    public void ChangeScene(GameScene scene)
    {
        SceneManager.LoadScene((int)scene);
        currentScene = scene;
    }
    /// <summary>
    /// Easy implementation of changing to the previous Scene
    /// TO-DO could be better with a tree like structure
    /// </summary>
    public void SceneBack()
    {
        int currentSceneIndex = (int)currentScene;
        
        if (currentScene > 0)
        {
            currentSceneIndex--;
            ChangeScene((GameScene)currentSceneIndex);
            
        }
        Debug.Log("Back");
    }
    /// <summary>
    /// This method should be called at the start of each scene by the SceneController, it updates the current SceneType and reevaluates
    /// the current Escape and Pause Events, since these mix each other inputs.
    /// </summary>
    /// <param name="sceneType"></param>
    public void ChangeCurrentSceneType(SceneType sceneType)
    {
        currentSceneType = sceneType;
        EvaluateEvents();
    }
    /// <summary>
    /// Joins Escape and Pause Events depending of the type of Scene there is. Should happen at the start of every Scene
    /// </summary>
    private void EvaluateEvents()
    {
        InputController.Instance.Escape = null;
        InputController.Instance.Pause = null;
        if (currentSceneType == SceneType.Screen)
        {
            InputController.Instance.Escape += SceneBack;
        }
        if (currentSceneType == SceneType.Level)
        {
            InputController.Instance.Pause += PauseGame;
        }
    }
    /// <summary>
    /// This is where the pause logic will go.
    /// </summary>
    public void PauseGame()
    {

    }
}

public enum GameScene
{
    MainScreen,
    SaveScreen,
    CharacterScreen,
    DifficultyScreen,
    testfelipe
}