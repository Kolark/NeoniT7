using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
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
        if (levelType == SceneType.Level)
        {
            int indexToSpawn = GameManager.Instance.Current.chamber;
            GameObject character = Instantiate(GameManager.Instance.Characters[(int)GameManager.Instance.Current.character],
                CheckPoints[indexToSpawn].position, Quaternion.identity);
            GameManager.Instance.SetLevel(currentLevel);
        }

        


        GameManager.Instance.ChangeCurrentSceneType(levelType);
        
        if (externalSounds.Length > 0)
        {
            AudioManager.Instance.ReceiveExternal(externalSounds);
            AudioManager.Instance.Play(ScreenAudio);
        }

        

    }

    public void NextLevel()
    {
        GameManager.Instance.SetLevel(nextLevel);
        GameManager.Instance.Save();
        GameManager.Instance.SetChamber(0);
        MenuManager.Instance.NextLevelTransition();
        DOVirtual.DelayedCall(5f,() => { GameManager.Instance.ChangeScene(GameManager.Instance.Current); });
        
    }

    public void GoToLastCheckpoint()
    {
        //Revive
        ChamberManager.Instance.UnlockPreviousChamber();
        BasicCharacter.Instance.transform.position = CheckPoints[GameManager.Instance.Current.chamber].position;
        BasicCharacter.Instance.Revive();
        DOVirtual.DelayedCall(0.25f, () => {
            ChamberManager.Instance.EnableTriggerExit();
        }).SetUpdate(true);
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