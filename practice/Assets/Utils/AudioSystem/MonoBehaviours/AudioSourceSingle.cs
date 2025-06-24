using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class AudioSourceSingle : BaseAudioSource
{
    protected override void Play(AudioData data)
    {
        TargetAudioSource.loop = data.Loop;
        TargetAudioSource.volume = data.Volume;
            

        if (data.OneShot == true)
        {
            TargetAudioSource.PlayOneShot(data.Clip);
        }
        else
        {
            TargetAudioSource.clip = data.Clip;
            TargetAudioSource.Play();
        }
    }

    protected override void UpdateMute(Volume volume)
    {
        if (TargetAudioSource.mute != volume.Mute)
        {
            TargetAudioSource.mute = volume.Mute;
            OnChangedMute();
        }
            
    }

}

