using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public abstract class BaseAudioPlayerAuto : BaseAudioPlayer
{
    public bool PlayOnEnable = true;

    private bool firstEnable = true;


    private void OnEnable()
    {
        if (PlayOnEnable == true && firstEnable == false) 
        {
            Play();
        }
    }

    private void Start()
    {
        if (firstEnable == true) 
        {
            firstEnable = false;
            if (PlayOnEnable == true)
            {
                Play();
            }
        }            
    }

    public abstract void Play();
        
}
