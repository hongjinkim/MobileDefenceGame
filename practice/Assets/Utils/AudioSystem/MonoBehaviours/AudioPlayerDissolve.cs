using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class AudioPlayerDissolve : BaseAudioPlayerAuto
{
    public AudioClip Clip;
    public float DissolveDuration = 2f;

    public override void Play() => Dissolve(Clip, DissolveDuration);

#if UNITY_EDITOR
    private void Reset()
    {
        if (Channel == MixerChannel.None)
        {
            Channel = MixerChannel.BGM;
            OnValidate();
        }
    }
#endif

}


