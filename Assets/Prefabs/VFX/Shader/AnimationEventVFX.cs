using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationEventVFX : MonoBehaviour
{
    [SerializeField] ParticleSystem ps;
    void AnimationOn()
    {
        ps.Play();
    }
}
