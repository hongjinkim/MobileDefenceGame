using System;
using UnityEngine;


[CreateAssetMenu(menuName = "Sound/AudioClip", fileName = "AudioClip")]
public class AudioClip : ScriptableObject
{
    public UnityEngine.AudioClip Clip;
    [Range(0f, 1f)]
    public float Volume = 0.8f;
}


