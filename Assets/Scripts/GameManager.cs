using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;
public class GameManager : MonoBehaviour
{
    private static GameManager instance;
    public static GameManager Instance => instance;

    private ScenesBuild currentScene;
    public ScenesBuild CurrentScene { get => currentScene;}

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            currentScene = ScenesBuild.MainScreen;
        }
        else
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);
    }
    private void Update()
    {
        if (Keyboard.current.escapeKey.wasPressedThisFrame)
        {
            SceneBack();
        }
    }
    public void ChangeScene(ScenesBuild scene)
    {
        SceneManager.LoadScene((int)scene);
        currentScene = scene;
    }

    public void SceneBack()
    {
        int currentSceneIndex = (int)currentScene;
        if (currentScene > 0)
        {
            currentSceneIndex--;
            ChangeScene((ScenesBuild)currentSceneIndex);
            
        }
    }
}

public enum ScenesBuild
{
    MainScreen,
    SaveScreen,
    CharacterScreen,
    DifficultyScreen,
    testfelipe
}