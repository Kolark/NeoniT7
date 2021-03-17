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
            if (particles[i].isPaused)
            {
                particles[i].Play();
            }
        }
    }

    public void StopEffect(int i)
    {
        particles[i].gameObject.SetActive(false);
    }


}
