using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VolumeController : MonoBehaviour
{
    public MixerChannel Channel = MixerChannel.None;
    public VolumeHandler VolumeHandler = null;

    public void Set(float sliderValue)
    {
        VolumeHandler.SetVolume(sliderValue);
    }

#if UNITY_EDITOR

    private void OnValidate()
    {
        if (Channel != MixerChannel.None)
        {
            try
            {
                string path = $"Assets/Utils/AudioSystem/Asset/Volume/{Channel}VolumeHandler.asset";
                var handler = UnityEditor.AssetDatabase.LoadAssetAtPath<VolumeHandler>(path);
                VolumeHandler = handler;
            }
            catch
            {

            }
        }
        else
        {
            VolumeHandler = null;
        }
    }
        
#endif


}


