using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// This class will refer to individual things that have need to be setup in each scene, therefore
/// this class will not be in the DontDestroyOnLoad Method and lastly it will not be a prefab.
/// </summary>
public class SceneController : MonoBehaviour
{
    /// <summary>
    /// Sounds exclusive to the currentScene    
    /// </summary>
    public Sound[] externalSounds;
    /// <summary>
    /// SceneType, this means if its a ui Screen or a Gameplay one.
    /// </summary>
    public SceneType levelType;
    [SerializeField] string ScreenAudio;
    public void Start()
    {
        GameManager.Instance.ChangeCurrentSceneType(levelType);
        if(externalSounds.Length > 0)
        {
            AudioManager.Instance.ReceiveExternal(externalSounds);
            AudioManager.Instance.Play(ScreenAudio);
        }
    }

}
public enum SceneType
{
    Screen,
    Level
}