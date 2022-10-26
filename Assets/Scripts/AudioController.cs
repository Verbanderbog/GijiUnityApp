using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class AudioController : MonoBehaviour
{
    public static AudioController i;
    public AudioMixer _MasterMixer;

    // Start is called before the first frame update
    void Awake()
    {
        InitializeMixer();
        if (i == null)
        {
            i = this;
        }
        else
        {
            DestroyImmediate(this);
        }
        
    }

    public void SetVolume(Slider volume)
    {
        switch (volume.name){
            case "Master Volume":
                SetMasterVolume(volume);
                break;
            case "Music Volume":
                SetMusicVolume(volume);
                break;
            case "SFX Volume":
                SetSFXVolume(volume);
                break;
        }
            
                
    }

    public void InitializeMixer()
    {
        _MasterMixer.SetFloat("Master", AdjustVolumeValue(PlayerPrefs.GetFloat("Master Volume")));
        _MasterMixer.SetFloat("Music", AdjustVolumeValue(PlayerPrefs.GetFloat("Music Volume")));
        _MasterMixer.SetFloat("SFX", AdjustVolumeValue(PlayerPrefs.GetFloat("SFX Volume")));
    }

    public void SetMasterVolume(Slider volume)
    {
        SetMasterVolume(volume.value);
    }
    public void SetMasterVolume(float volume)
    {
        PlayerPrefs.SetFloat("Master Volume", volume);
        _MasterMixer.SetFloat("Master", AdjustVolumeValue(volume));
    }
    public void SetMusicVolume(Slider volume)
    {
        SetMusicVolume(volume.value);
    }
    public void SetMusicVolume(float volume)
    {
        PlayerPrefs.SetFloat("Music Volume", volume);
        _MasterMixer.SetFloat("Music", AdjustVolumeValue(volume));
    }
    public void SetSFXVolume(Slider volume)
    {
        SetSFXVolume(volume.value);
    }
    public void SetSFXVolume(float volume)
    {
        PlayerPrefs.SetFloat("SFX Volume", volume);
        _MasterMixer.SetFloat("SFX", AdjustVolumeValue(volume));
    }

    private float AdjustVolumeValue(float volume)
    {
        return Mathf.Log10(volume) * 20;
    }
}
