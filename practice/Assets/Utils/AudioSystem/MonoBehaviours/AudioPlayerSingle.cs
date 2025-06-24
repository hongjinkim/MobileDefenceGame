using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class AudioPlayerSingle : BaseAudioPlayerAuto
{
    public AudioClip Clip;

    public override void Play() => Play(Clip);
}


