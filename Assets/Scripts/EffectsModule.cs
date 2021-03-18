using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class EffectsModule : MonoBehaviour
{
    [SerializeField] List<ParticleSystem> particles;
    public void PlayEffect(int i)
    {
        if (particles[i].main.loop)
        {
            particles[i].gameObject.SetActive(true);
        }
        else
        {
            if (!particles[i].isPlaying)
            {
                particles[i].Play(true);
            }
        }
    }

    public void StopEffect(int i)
    {
        particles[i].gameObject.SetActive(false);
    }


}
