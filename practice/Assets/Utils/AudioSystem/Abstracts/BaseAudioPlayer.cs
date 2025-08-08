using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class BaseAudioPlayer : MonoBehaviour
{
    [SerializeField]
    public virtual MixerChannel Channel { get; set; }
    public AudioHandler AudioHandler;

    public void Play(AudioClip clip)
    {
        if (AudioHandler == null) 
        {
            Debug.LogError("[!] Audio Handler is null");
            return;
        }
        else if (clip == null)
        {
            Debug.LogError("[!] Audio Clip is null");
            return;
        }

        AudioHandler.Play(clip);
    }

    public void Dissolve(AudioClip clip, float dissolveDurarion)
    {
        if (AudioHandler == null)
        {
            Debug.LogError("[!] Audio Handler is null");
            return;
        }
        else if (clip == null)
        {
            Debug.LogError("[!] Audio Clip is null");
            return;
        }

        AudioHandler.Dissolve(clip, dissolveDurarion);
    }
        

#if UNITY_EDITOR
    protected void OnValidate()
    {
        if (Channel != MixerChannel.None)
        {
            try
            {
                string path = $"Assets/Utils/AudioSystem/Asset/Audio/{Channel}AudioHandler.asset";
                var handler = UnityEditor.AssetDatabase.LoadAssetAtPath<AudioHandler>(path);
                AudioHandler = handler;
            }
            catch
            {

            }
        }
        else
        {
            AudioHandler = null;
        }

    }
#endif
}


