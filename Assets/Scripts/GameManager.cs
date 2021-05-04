using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;
using System;
/// <summary>
/// The GameManager is in charge of the overall Game Behaviour. Not specific to one scene, but rather all of them. 
/// Stuff like saving the game. Constructing a new save SaveInfo. Going between scenes.
/// 
/// Will primarily deal the overall in game information. Such as the current Scene(not level).
/// Basically transition between scenes and information that is relevant to the savesystem;
/// </summary>
public class GameManager : MonoBehaviour
{
    [SerializeField] GameObject[] characters;
    private static GameManager instance;
    public static GameManager Instance => instance;

    private GameScene currentScene;
    public GameScene CurrentScene { get => currentScene; }

    private SceneType currentSceneType;
    private ControllerType controllerType { get => InputController.Instance.CurrentControlScheme; }

    private SaveInfo current;
    bool hasSaveInfo = false;
    bool isPaused = false;

    private int saveSlot;
    public int Saveslot { 
        get => saveSlot;
        set
        {
            saveSlot = Mathf.Clamp(value, 0, SaveSystem.SavesNumber - 1);
        }
    }

    public SaveInfo Current { get => current; set => current = value; }
    public bool IsPaused { get => isPaused;}
    public GameObject[] Characters { get => characters;}


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
        SaveSystem.Init();
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
    public void ChangeScene(SaveInfo saveInfo,bool canSave = false)
    {
        if (canSave)
        {
            Save(saveInfo);
        }
        SceneManager.LoadScene((int)saveInfo.currentScene);
        current = saveInfo;
        currentScene = current.currentScene;
    }
    public void Save()
    {
        SaveSystem.Save(current);
    }
    public void Save(SaveInfo saveInfo)
    {
        SaveSystem.Save(saveInfo);
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
        //InputController.Instance.Pause = null;
        if (currentSceneType == SceneType.Screen)
        {
            InputController.Instance.Escape += SceneBack;
        }
        //if (currentSceneType == SceneType.Level)
        //{
        //    InputController.Instance.Pause += PauseGame;
        //}
    }

    public void Unpause()
    {
        isPaused = false;
        Time.timeScale = 1;
    }
    public void DisableCharacter()
    {
        //Disables all player input on the Character
    }
    public void EnableCharacter()
    {
        //Reenables all playeri nput on the Character
    }
    public void Pause()
    {
        isPaused = true;
        Time.timeScale = 0;
    }

    /// <summary>
    /// Restart the current Lvl at Chamber 0
    /// </summary>
    public void Restart()
    {
        current.chamber = 0 ;
        Unpause();
        Save();
        ChangeScene(SceneController.Instance.CurrentLevel);
    }
    /// <summary>
    /// Changes the scene to the currentScene
    /// </summary>
    public void MainScreen()
    {
        Unpause();
        Save();
        ChangeScene(GameScene.MainScreen);
    }

    public void SetChamber(int currentChamber)
    {
        current.chamber = currentChamber;
    }
    public void SetLevel(GameScene currentLevel)
    {
        current.currentScene = currentLevel;
    }
    public void SetCharacter(CharacterType character)
    {
        current.character = character;
    }
    public void SetDifficulty(Difficulty difficulty)
    {
        current.difficulty = difficulty;
    }

    public void SetScore(int score, float timeSpent)
    {
        current.currentScore = score;
        current.timeSpentInLevel = timeSpent;
    }

    #if UNITY_EDITOR
    [Header("EDITOR STUFF")]
    [SerializeField] public SaveInfo SaveInfoEDITOR;

    #endif
}

public enum Difficulty
{
    Normal,
    Hardcore
}

public enum CharacterType
{
    Samurai,
    Ninja,
    Yakuza
}

public enum GameScene
{
    MainScreen,
    SaveScreen,
    ComicIntroductionScreen,
    CharacterScreen,
    DifficultyScreen,
    Level1,
    Level2,
    Level3,
    Level4,
    Level5
}