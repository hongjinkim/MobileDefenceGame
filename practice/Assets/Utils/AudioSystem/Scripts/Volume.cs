using System;
using UnityEngine;


[Serializable]
public class Volume
{
    public float Value;
    public float DB;
        
    public bool Mute => mute || forceMute;

    private bool forceMute = false;
    private bool mute = false;


    public void ForceMute(bool mute, IAudioVolumeListener listener)
    {
        if (forceMute != mute)
        {
            forceMute = mute;
            listener.Listen(this);
        }
    }

    public void SetVolume(float sliderValue, IAudioVolumeListener listener)
    {
        Value = Mathf.Clamp01(sliderValue);

        if (sliderValue < 0.0001f)
        {
            mute = true;
            DB = -80f;
        }
        else
        {
            mute = false;
            DB = Mathf.Log10(sliderValue) * 20;
        }

        listener?.Listen(this);
    }
}

