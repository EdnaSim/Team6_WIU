using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class AudioVolumeManager : MonoBehaviour
{
    public AudioMixer mixer;
    public void Start()
    {
        // Load the saved Master volume from PlayerPrefs and apply it to the mixer
        float masterVolume = PlayerPrefs.GetFloat("Master Vol", 1f); // Default to 1 if not found
        mixer.SetFloat("Master Vol", VolumeToDecibels(masterVolume));

        // Load the saved Music volume from PlayerPrefs and apply it to the mixer
        float musicVolume = PlayerPrefs.GetFloat("Music Vol", 1f); // Default to 1 if not found
        mixer.SetFloat("Music Vol", VolumeToDecibels(musicVolume));

        // Load the saved SFX volume from PlayerPrefs and apply it to the mixer
        float sfxVolume = PlayerPrefs.GetFloat("SFX Vol", 1f); // Default to 1 if not found
        mixer.SetFloat("SFX Vol", VolumeToDecibels(sfxVolume));
    }

    public void SetMasterVolume(float volume)
    {
        mixer.SetFloat("Master Vol", VolumeToDecibels(volume));
        PlayerPrefsManager.Save("Master Vol", volume);
    }

    public void SetMusicVolume(float volume)
    {
        mixer.SetFloat("Music Vol", VolumeToDecibels(volume));
        PlayerPrefsManager.Save("Music Vol", volume);
    }

    public void SetSFXVolume(float volume)
    {
        mixer.SetFloat("SFX Vol", VolumeToDecibels(volume));
        PlayerPrefsManager.Save("SFX Vol", volume);
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

    public float GetMasterVolume()
    {
        float volume;
        mixer.GetFloat("Master Vol", out volume);
        return DecibelsToVolume(volume);
    }

    // Function to get the current Music Volume from the mixer
    public float GetMusicVolume()
    {
        float volume;
        mixer.GetFloat("Music Vol", out volume);
        return DecibelsToVolume(volume);
    }

    // Function to get the current SFX Volume from the mixer
    public float GetSFXVolume()
    {
        float volume;
        mixer.GetFloat("SFX Vol", out volume);
        return DecibelsToVolume(volume);
    }

    // Method to stop the SFX audio
    public void StopSFXAudio()
    {
        mixer.SetFloat("SFX Vol", VolumeToDecibels(0f));
    }

    // Method to resume the SFX audio
    public void ResumeSFXAudio()
    {
        // Get the previously set SFX volume from PlayerPrefs and apply it
        float sfxVolume = PlayerPrefs.GetFloat("SFX Vol", 1f); // Default to 1 if not found
        mixer.SetFloat("SFX Vol", VolumeToDecibels(sfxVolume));
    }
}