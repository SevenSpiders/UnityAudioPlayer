using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class AudioPlayer : MonoBehaviour
{

    // Global Audio Volumes:
    public static float VOLUME = 1f;
    public static float VOLUME_MUSIC = 1f;
    public static float VOLUME_SFX = 1f;


    public enum PlayerType {
        SFX = 0,
        Music = 1,
        Other = 2,
    }

    [Range(0f, 1f), SerializeField] float _volume = 1f;
    public List<Sound> sounds;
    public PlayerType playerType;
    [SerializeField] GameObject audioContainer;




    public float volume => playerType switch {
        PlayerType.SFX => VOLUME * VOLUME_SFX *_volume,
        PlayerType.Music => VOLUME * VOLUME_MUSIC * _volume,
        PlayerType.Other => VOLUME * _volume,
        _ => VOLUME
    };

    
    void Awake() {
        if (!audioContainer) audioContainer = this.gameObject;
        foreach (Sound sound in sounds) 
            CreateAudioComponent(sound);
    }

    void Start() {
        foreach (Sound sound in sounds) sound.Start();
    }

    

    void CreateAudioComponent(Sound sound) {
        sound.audioPlayer = this;
        sound.source = audioContainer.AddComponent<AudioSource>();
        sound.UpdateClip();
        sound.UpdateSource();
        sound.source.spatialBlend = 0f; // important else no audio falloff in distance
        sound.source.maxDistance = 20f;
        sound.source.rolloffMode = AudioRolloffMode.Linear;
        sound.source.loop = sound.loop;
    } 
    

    public void AddSound(Sound sound) {
        sounds.Add(sound);
        CreateAudioComponent(sound);
    }

    public void Play() {    // For audioplayers with only one sound effect
        if (sounds.Count == 0) return;
        sounds[0].Play();
    }

    public void Play(AudioClip audioClip) {
        if (audioClip == null) return;
        if (sounds.Count == 0) return;
        sounds[0].PlayOneShot(audioClip);
    }
    
    
    public Sound Play(string name, bool oneShot = false) {
        foreach (Sound s in sounds) {
            if (s.name == name) {
                if (s.clip == null) continue;
                if (oneShot) s.PlayOneShot();
                else s.Play();
                return s;
            }
        }
        return null;
    }
    
   

    public Sound Play(string name, AudioParameters parameters)  {
        foreach (Sound s in sounds) {
            if (s.name == name)  {
                s.SetParameters(parameters);
                s.Play();
                return s;
            }
        }
        return null;
    }


    public Sound PlayRandom(string name, bool oneShot = false) {

        List<Sound> soundList = new();
        foreach (Sound s in sounds) if (s.name == name) soundList.Add(s);

        if (soundList.Count == 0) return null;

        int idx = Random.Range(0, soundList.Count);
        if (oneShot) soundList[idx].PlayOneShot();
        else soundList[idx].Play();
        return soundList[idx];
    }

    public void Stop(string name) {
        foreach (var s in sounds) {
            if (s.name == name) s.source.Stop();
        }
    }


    public Sound FindSound(string name) {
        for (int i=0; i< sounds.Count; i++) {
            if (sounds[i].name == name) return sounds[i];
        }
        return null;
    }


    void OnValidate() {
        if (sounds == null) return;
        foreach(var sound in sounds) {
            if (sound.pitch == 0 && sound.volume == 0) {
                sound.SetPitchVolume(1f, 1f);
            }
        }
    }

}

