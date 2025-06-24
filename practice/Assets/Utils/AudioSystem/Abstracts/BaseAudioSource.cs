using System;
using UnityEngine;


public abstract class BaseAudioSource : MonoBehaviour
{
    public event Action<bool> OnMuteChanged;

    public VolumeHandler VolumeHandler;
    public AudioHandler AudioHandler;
    public AudioSource TargetAudioSource = null;


    protected virtual void OnEnable()
    {
        VolumeHandler.OnUpdate += UpdateMute;
        AudioHandler.OnClipReady += Play;
    }

    protected virtual void OnDisable()
    {
        VolumeHandler.OnUpdate -= UpdateMute;
        AudioHandler.OnClipReady -= Play;
    }


    protected void OnChangedMute() => OnMuteChanged?.Invoke(TargetAudioSource.mute);

    protected abstract void Play(AudioData data);

    protected abstract void UpdateMute(Volume volume);

}


