using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class AudioSourceCrossfadable : BaseAudioSource
{
    public VolumeHandler MainVolumeHandler;
    public AudioSource SubAudioSource;
    public VolumeHandler SubVolumeHandler;

    private AudioSource current = null;
    private AudioSource next = null;
    private Coroutine onDissolving = null;


    protected override void OnEnable()
    {
        base.OnEnable();
        AudioHandler.OnCrossFade += Dissolve;
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        AudioHandler.OnCrossFade -= Dissolve;
    }


    protected override void Play(AudioData data)
    {
        if (current == null)
        {
            current = TargetAudioSource;
            next = SubAudioSource;
        }

        current.loop = data.Loop;
        current.volume = data.Volume;
            

        if (data.OneShot == true)
        {
            current.PlayOneShot(data.Clip);
        }
        else
        {
            current.clip = data.Clip;
            current.Play();
        }
    }

    protected override void UpdateMute(Volume volume)
    {
        if (TargetAudioSource.mute != volume.Mute)
        {
            TargetAudioSource.mute = volume.Mute;
            SubAudioSource.mute = volume.Mute;
            OnChangedMute();
        }
    }


    private void Dissolve(AudioData data, float duration)
    {
        if (onDissolving != null)
        {
            StopCoroutine(onDissolving);
            onDissolving = null;
        }

        if (current == null)
        {
            next = TargetAudioSource;
            current = SubAudioSource;
        }

        var outVolume = current.Equals(TargetAudioSource) ? MainVolumeHandler : SubVolumeHandler;
        var inVolume = next.Equals(SubAudioSource) ? SubVolumeHandler : MainVolumeHandler;

        next.clip = data.Clip;
        next.loop = data.Loop;
        next.volume = data.Volume;
        next.Play();

        onDissolving = StartCoroutine(Crossfade(outVolume, inVolume, duration));
    }

    private IEnumerator Crossfade(VolumeHandler outVolume, VolumeHandler inVolume, float duration)
    {

        float t = 0;
        while (duration > 0 && t < 1)
        {
            t += Time.deltaTime / duration;
            outVolume.SetVolume(1 - t);
            inVolume.SetVolume(t);
            yield return null;
        }

        outVolume.SetVolume(0);
        inVolume.SetVolume(1);

        if (current.isPlaying == true) current.Stop();
            
        (current, next) = (next, current);

        onDissolving = null;
    }

}


