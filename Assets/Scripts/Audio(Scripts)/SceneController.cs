using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
/// <summary>
/// This class will refer to individual things that have need to be setup in each scene, therefore
/// this class will not be in the DontDestroyOnLoad Method and lastly it will not be a prefab.
/// </summary>
public class SceneController : MonoBehaviour
{

    private static SceneController instance;
    public static SceneController Instance { get => instance; }

    /// <summary>
    /// Sounds exclusive to the currentScene    
    /// </summary>
    public Sound[] externalSounds;
    /// <summary>
    /// SceneType, this means if its a ui Screen or a Gameplay one.
    /// </summary>
    public SceneType levelType;
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
        GameManager.Instance.ChangeCurrentSceneType(levelType);
        if(externalSounds.Length > 0)
        {
            AudioManager.Instance.ReceiveExternal(externalSounds);
            AudioManager.Instance.Play(ScreenAudio);
        }
    }

    public void NextLevel()
    {
        GameManager.Instance.SetLevel(nextLevel);
        GameManager.Instance.Save();
        DOVirtual.DelayedCall(5f,() => { GameManager.Instance.ChangeScene(GameManager.Instance.Current); });
        
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