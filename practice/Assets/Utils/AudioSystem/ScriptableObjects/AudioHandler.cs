using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

[CreateAssetMenu(menuName = "Sound/AudioHandler", fileName = "AudioHandler")]
public class AudioHandler : ScriptableObject
{
    public event Action<AudioData> OnClipReady;
    public event Action<AudioData, float> OnCrossFade;

    [Min(0f)]
    public float ThresholdSeconds = 0.1f;
    public bool Loop = false;
    public bool OneShot = true;

    private readonly List<UnityEngine.AudioClip> playingList = new();


    public void Dissolve(AudioClip clip, float duration)
    {
        Debug.Log("Dissolve Sound");

        var data = new AudioData
        {
            Clip = clip.Clip,
            Volume = clip.Volume,
            Loop = Loop,
            OneShot = OneShot,
               
        };

        OnCrossFade?.Invoke(data, duration);
    }

    public void Play(AudioClip clip)
    {
        if (ThresholdSeconds > 0f && playingList.Contains(clip.Clip) == true) return;

        var data = new AudioData
        {
            Clip = clip.Clip,
            Volume = clip.Volume,
            Loop = Loop,
            OneShot = OneShot,
        };

        if (ThresholdSeconds > 0f) Hold(data.Clip);

        OnClipReady?.Invoke(data);
    }


    private async void Hold(UnityEngine.AudioClip clip)
    {
        playingList.Add(clip);
        await Task.Delay(Mathf.FloorToInt(ThresholdSeconds * 1000));
        playingList.Remove(clip);
    }

}


