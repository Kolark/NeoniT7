using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Cinemachine;
using System;
using UnityEngine.UI;
/// <summary>
/// This class will refer to individual things that have need to be setup in each scene, therefore
/// this class will not be in the DontDestroyOnLoad Method and lastly it will not be a prefab.
/// 
/// This class will be the one that communicates more to the gameManager.
/// 
/// Will primarily hold information about the current scene.
/// </summary>
public class SceneController : MonoBehaviour
{

    private static SceneController instance;
    public static SceneController Instance { get => instance; }
    public GameScene CurrentLevel { get => currentLevel;}

    [SerializeField] Transform[] CheckPoints;

    /// <summary>
    /// Sounds exclusive to the currentScene    
    /// </summary>
    public Sound[] externalSounds;
    /// <summary>
    /// SceneType, this means if its a ui Screen or a Gameplay one.
    /// </summary>
    public SceneType levelType;
    [SerializeField] GameScene currentLevel;
    [SerializeField] GameScene nextLevel;
    [SerializeField] string ScreenAudio;

    [Header("Level Info")]
    [SerializeField] string levelName;
    [Header("endingAnimation")]
    [SerializeField] Transform train;
    [SerializeField] CinemachineVirtualCamera endingAnimationVirtualCam;
    [SerializeField] Transform[] spritePersonajes;

    public Transform Train { get => train;}
    public CinemachineVirtualCamera EndingAnimationVirtualCam { get => endingAnimationVirtualCam;}
    public Transform[] SpritePersonajes { get => spritePersonajes;}

    [SerializeField] int normalLevelLifes;

    private int levelLifes;
    public int LevelLifes { get => levelLifes;}


    public Action OnLifeLost;
    [Header("DeathAnimation")]
    [SerializeField] GameObject DeathAnimation;
    [SerializeField] Image[] images;
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
    }
    
    public void Start()
    {
        GameManager.Instance.Unpause();
        
        if(Comic.Instance != null)
        {
            if(levelType == SceneType.Level)
            {
                Comic.Instance.INIT_COMIC(() => { MenuManager.Instance.StartLevelTransition(levelName); });
            }
            else
            {
                Comic.Instance.INIT_COMIC(() => { NextLevel(); });
            }
        }


        if (levelType == SceneType.Level)
        {
            
            int indexToSpawn = GameManager.Instance.Current.chamber;
            GameObject character = Instantiate(GameManager.Instance.Characters[(int)GameManager.Instance.Current.character],
                CheckPoints[indexToSpawn].position, Quaternion.identity);
            GameManager.Instance.SetLevel(currentLevel);
            
            //MenuManager.Instance.StartLevelTransition(levelName);
            BasicCharacter.Instance.OnCharacterDeath += OnPlayersDeath;

        }
        GameManager.Instance.ChangeCurrentSceneType(levelType);
        
        if (externalSounds.Length > 0)
        {
            AudioManager.Instance.ReceiveExternal(externalSounds);
            AudioManager.Instance.Play(ScreenAudio);
        }

        if(levelType == SceneType.Level)
        {
            Debug.Log("gamemmanagerlifes : " + GameManager.Instance.Current.lifes);
            Debug.Log("level : " + levelLifes);
            if (GameManager.Instance.Current.lifes > normalLevelLifes)
            {
                if (GameManager.Instance.Current.difficulty == Difficulty.Hardcore)
                {
                    levelLifes = 1;
                }
                else
                {
                    levelLifes = normalLevelLifes;
                }
            }
            else
            {
                levelLifes = GameManager.Instance.Current.lifes;
            }
        }
    }
    void OnPlayersDeath()
    {
        levelLifes--;
        GameManager.Instance.SetLifes(levelLifes);
        GameManager.Instance.Save();
        bool hasLost = levelLifes <= 0;
        if (hasLost)
        {
            DeathAnimation.SetActive(true);
            //for (int i = 0; i < images.Length; i++)
            //{
            //    images[i].color = Color.clear;
            //    images[i].DOColor(Color.white, 5f);
            //}
            //images[0].color = Color.clear;
            //images[0].DOColor(Color.black, 5f);
            images[1].color = Color.clear;
            images[1].DOColor(Color.white, 5f);
            BasicCharacter.Instance.Deactivate();
            DOVirtual.DelayedCall(8f, () => 
            {
                DeathAnimation.SetActive(false);
                BasicCharacter.Instance.Activate();
                levelLifes = normalLevelLifes;
                OnLifeLost?.Invoke();
                GameManager.Instance.SetLifes(levelLifes);
                GameManager.Instance.Restart();
            });
        }
        OnLifeLost?.Invoke();
    }
    public void NextLevel()
    {
        GameManager.Instance.SetLevel(nextLevel);
        GameManager.Instance.Save();
        GameManager.Instance.SetChamber(0);
        if(MenuManager.Instance != null)
        {
            MenuManager.Instance.NextLevelTransition(() => { GameManager.Instance.ChangeScene(GameManager.Instance.Current); });
        }
        else
        {
            GameManager.Instance.ChangeScene(GameManager.Instance.Current);
        }
    }

    public void GoToLastCheckpoint()
    {
        //Revive
        Debug.Log("Lat Checkpoint");
        ChamberManager.Instance.UnlockPreviousChamber();
        ScoreManager.Instance.ResetUnsavedValues();
        BasicCharacter.Instance.transform.position = CheckPoints[GameManager.Instance.Current.chamber].position;
        BasicCharacter.Instance.Revive();
        DOVirtual.DelayedCall(0.25f, () => {
            ChamberManager.Instance.EnableTriggerExit();
        }).SetUpdate(true);
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    private void OnDestroy()
    {
        if (instance != this)
        {
            instance = null;
        }
    }
}
public enum SceneType
{
    Screen,
    Level
}