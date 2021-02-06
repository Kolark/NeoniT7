using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// This class will refer to individual things that have need to be setup in each scene, therefore
/// this class will not be in the DontDestroyOnLoad Method and lastly it will not be a prefab.
/// </summary>
public class SceneController : MonoBehaviour
{
    public Sound[] externalSounds;

    public void Start()
    {
        AudioManager.Instance.ReceiveExternal(externalSounds);
    }

    private void OnValidate()
    {
        if (externalSounds.Length > 0)
        {
            for (int i = 0; i < externalSounds.Length; i++)
            {
                if (externalSounds[i].clip != null)
                {
                    externalSounds[i].name = externalSounds[i].clip.name;
                }
            }
        }
    }

}

public enum scenes
{
    MainScreen
}