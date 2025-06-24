using System;
using UnityEngine;
using UnityEngine.Audio;


[CreateAssetMenu(menuName = "Sound/VolumeHandler", fileName = "VolumeHandler")]
public class VolumeHandler : ScriptableObject, IAudioVolumeListener
{
    public event Action<Volume> OnUpdate;

    public string VolumeName;
    public AudioMixer Mixer;

    private readonly Volume volume = new();


    public void Listen(Volume volume)
    {
        Mixer.SetFloat(VolumeName, volume.DB);
        OnUpdate?.Invoke(volume);
    }

    public void SetVolume(float sliderValue)
    {
        volume.SetVolume(sliderValue, this);
    }

}

