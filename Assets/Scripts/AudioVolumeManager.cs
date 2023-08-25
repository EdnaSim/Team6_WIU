using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class AudioVolumeManager : MonoBehaviour
{
    public AudioMixer mixer;

    // Start is called before the first frame update
    public void Start()
    {
        // Load and apply the saved Master volume from PlayerPrefs
        float masterVolume = PlayerPrefs.GetFloat("Master Vol", 1f); // Default to 1 if not found
        mixer.SetFloat("Master Vol", VolumeToDecibels(masterVolume));

        // Load and apply the saved Music volume from PlayerPrefs
        float musicVolume = PlayerPrefs.GetFloat("Music Vol", 1f); // Default to 1 if not found
        mixer.SetFloat("Music Vol", VolumeToDecibels(musicVolume));

        // Load and apply the saved SFX volume from PlayerPrefs
        float sfxVolume = PlayerPrefs.GetFloat("SFX Vol", 1f); // Default to 1 if not found
        mixer.SetFloat("SFX Vol", VolumeToDecibels(sfxVolume));
    }

    public void SetMasterVolume(float volume)
    {
        // Set the Master volume in the mixer and save it to PlayerPrefs
        mixer.SetFloat("Master Vol", VolumeToDecibels(volume));
        PlayerPrefsManager.Save("Master Vol", volume);
    }

    public void SetMusicVolume(float volume)
    {
        // Set the Music volume in the mixer and save it to PlayerPrefs
        mixer.SetFloat("Music Vol", VolumeToDecibels(volume));
        PlayerPrefsManager.Save("Music Vol", volume);
    }

    public void SetSFXVolume(float volume)
    {
        // Set the SFX volume in the mixer and save it to PlayerPrefs
        mixer.SetFloat("SFX Vol", VolumeToDecibels(volume));
        PlayerPrefsManager.Save("SFX Vol", volume);
    }

    // Function to get the current Master Volume from the mixer
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