using UnityEngine;
using UnityEngine.Audio;

[System.Serializable]
public class Sound
{
    #region variables
    public AudioClip clip;
    public MixerChannel mixerChannel;
    [Range(0, 1f)]
    public float volume= 1;
    [Range(.1f, 3f)]
    public float pitch = 1;
    public bool loop;
    [HideInInspector]
    public AudioSource source;
    #endregion
    /// <summary>
    /// Ayuda a organizar los parámetros de esta clase, ademas del audioSource
    /// </summary>
    /// <param name="source"></param>
    public void SetAudioSource(ref AudioSource source,ref AudioMixerGroup mixer)
    {
        this.source = source;
        this.source.clip = clip;
        this.source.volume = volume;
        this.source.pitch = pitch;
        this.source.loop = loop;
        this.source.outputAudioMixerGroup = mixer;
    }

}