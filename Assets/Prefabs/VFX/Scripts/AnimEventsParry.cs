using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimEventsParry : MonoBehaviour
{
    [SerializeField] ParticleSystem ps_1;
    [SerializeField] ParticleSystem ps_2;
    //[SerializeField] Animator anim;
    void Start()
    {

    }
    void Update()
    {
        //if (Input.GetButtonDown("Fire1"))
        //{
        //    anim.SetBool("click",true);
        //}
    }

    void SlashOne()
    {
        ps_1.Play();
    }
    void SlashTwo()
    {
        ps_2.Play();
    }

}
