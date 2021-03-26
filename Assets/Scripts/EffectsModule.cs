using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class EffectsModule : MonoBehaviour
{
    [SerializeField] List<ParticleSystem> particles;
    [SerializeField] Transform jumpPos;
    ParticleSystem jump;
    public void PlayEffect(int i)
    {
        if (particles[i].main.loop)
        {
            particles[i].gameObject.SetActive(true);
        }
        else
        {
            if (!particles[i].isPlaying && particles[i].name != "JumpParticleSystem")
            {
                particles[i].Play(true);
            }else if(!particles[i].isPlaying && particles[i].name == "JumpParticleSystem")
            {
                if(jump == null)
                {
                    jump = Instantiate(particles[i], jumpPos.position, jumpPos.rotation);
                }
                else
                {
                    jump.transform.position = jumpPos.position;
                    jump.transform.rotation = jumpPos.rotation;
                    jump.Play(true);
                }
            }
        }
    }

    public void StopEffect(int i)
    {
        particles[i].gameObject.SetActive(false);
    }


}
