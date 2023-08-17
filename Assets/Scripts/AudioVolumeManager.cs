using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class AudioVolumeManager : MonoBehaviour
{
    public AudioMixer mixer;
    public void SetMusicVolume(float volume)
    {
        mixer.SetFloat("MusicVolume", VolumeToDecibels(volume));
        PlayerPrefsManager.Save("MusicVolume", volume);
    }

    public void SetSFXVolume(float volume)
    {
        mixer.SetFloat("SFXVolume", VolumeToDecibels(volume));
        PlayerPrefsManager.Save("SFXVolume", volume);
    }

    // Function to convert linear volume to decibel scale
    private float VolumeToDecibels(float volume)
    {
        return 20f * Mathf.Log10(Mathf.Clamp(volume, 0.0001f, 1f));
    }

    // Function to convert decibel scale to linear volume
    private float DecibelsToVolume(float decibels)
    {
        return Mathf.Pow(10f, decibels / 20f);
    }

    // Function to get the current Music Volume from the mixer
    public float GetMusicVolume()
    {
        float volume;
        mixer.GetFloat("MusicVolume", out volume);
        return DecibelsToVolume(volume);
    }

    // Function to get the current SFX Volume from the mixer
    public float GetSFXVolume()
    {
        float volume;
        mixer.GetFloat("SFXVolume", out volume);
        return DecibelsToVolume(volume);
    }

    // Method to stop the SFX audio
    public void StopSFXAudio()
    {
        mixer.SetFloat("SFXVolume", VolumeToDecibels(0f));
    }

    // Method to resume the SFX audio
    public void ResumeSFXAudio()
    {
        // Get the previously set SFX volume from PlayerPrefs and apply it
        float sfxVolume = PlayerPrefsManager.Load("SFXVolume");
        mixer.SetFloat("SFXVolume", VolumeToDecibels(sfxVolume));
    }
}