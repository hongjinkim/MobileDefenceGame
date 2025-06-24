using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioPlayerRandom : BaseAudioPlayerAuto
{
    public AudioClip[] Clips;

    public override void Play() => Play(Clips[UnityEngine.Random.Range(0, Clips.Length)]);

}


