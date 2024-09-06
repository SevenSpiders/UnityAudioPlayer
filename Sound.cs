using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class Sound
{
    
    string Name;
    public string name;
    public AudioClip clip;

    public AudioDetails details;

    public float volume => details.volume;
    public float pitch => details.pitch;
    public bool loop => details.loop;
    public float duration => clip.length;
    
    [HideInInspector] public AudioPlayer audioPlayer; // owner
    [HideInInspector] public AudioSource source;

    public void Start() {
        if (details.playOnStart) Play();
    }


    public void Play() {
        UpdateSource();
        source.Play();
    }
    public void PlayOneShot() {
        UpdateSource();
        source.PlayOneShot(clip);
    }
    public void PlayOneShot(AudioClip audioClip) {
        UpdateSource();
        source.PlayOneShot(audioClip);
    }

    public void UpdateClip() {
        source.clip = clip;
    }

    public void UpdateSource() {
        source.volume = details.volume * audioPlayer.volume;
        source.pitch = details.pitch;
    }

    public void SetParameters(AudioParameters parameters) {
        SetPitchVolume(parameters.pitch, parameters.volume);
    }

    public void SetPitchVolume(float _pitch, float _volume) {
        details.pitch = _pitch;
        details.volume = _volume;
    }

    public void SetVolume(float _volume) {
        details.volume = _volume;
        source.volume = _volume;
    }

    public void SetPitch(float _pitch) {
        details.pitch = _pitch;
    }
}

[System.Serializable]
public class AudioDetails {
    
    public float volume;
    public float pitch;
    public bool loop;
    public bool playOnStart;
}
