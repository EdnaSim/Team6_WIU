using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    private GameObject activeMenu; // Reference to the currently active menu
    //public PlayerData playerData; // Reference to the PlayerData scriptable object

    public AudioVolumeManager audioVolumeManager;
    public Slider musicSlider; // Reference to the music slider in the UI
    public Slider sfxSlider; // Reference to the SFX slider in the UI

    public GameObject winMenu; // Reference to the "Win Menu" GameObject
    public GameObject loseMenu; // Reference to the "Lose Menu" GameObject

    private void Start()
    {
        HideMenu(); // Ensure the menu is hidden when the scene starts

        // Load player data when the game starts
        //playerData.Load();

        // Load and set music and SFX slider values from PlayerPrefs
        musicSlider.value = PlayerPrefsManager.Load("MusicVolume");
        sfxSlider.value = PlayerPrefsManager.Load("SFXVolume");

        // Add listeners to music and SFX sliders to update mixer volumes
        musicSlider.onValueChanged.AddListener(UpdateMusicVolume);
        sfxSlider.onValueChanged.AddListener(UpdateSFXVolume);
    }

    private void UpdateMusicVolume(float volume)
    {
        audioVolumeManager.SetMusicVolume(volume);
    }

    private void UpdateSFXVolume(float volume)
    {
        audioVolumeManager.SetSFXVolume(volume);
        // If there is an active menu, hide it before showing the "Win Menu"
        if (activeMenu != null)
        {
            // Stop the SFX audio
            audioVolumeManager.StopSFXAudio();
        }
    }

    public void PlayGame()
    {
        // Reset the time scale to 1 before loading the next scene
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    // Function to reset the player data and s
    public void NewGame()
    {
        //playerData.Reset();
        // Reset the time scale to 1 before loading the next scene
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void ShowMenu(GameObject menu)
    {
        // If there is an active menu, hide it before showing the new one
        if (activeMenu != null)
        {
            HideMenu();
        }

        // Pause the game by setting time scale to 0
        Time.timeScale = 0f;

        // Stop the SFX audio
        audioVolumeManager.StopSFXAudio();

        // Show the requested menu
        menu.SetActive(true);
        activeMenu = menu;
    }

    public void ShowWinMenu()
    {
        // If there is an active menu, hide it before showing the "Win Menu"
        if (activeMenu != null)
        {
            HideMenu();
        }

        // Pause the game by setting time scale to 0
        Time.timeScale = 0f;

        // Stop the SFX audio
        audioVolumeManager.StopSFXAudio();

        // Show the "Win Menu"
        winMenu.SetActive(true);
        activeMenu = winMenu;
    }

    public void ShowLoseMenu()
    {
        // If there is an active menu, hide it before showing the "Lose Menu"
        if (activeMenu != null)
        {
            HideMenu();
        }

        // Pause the game by setting time scale to 0
        Time.timeScale = 0f;

        // Stop the SFX audio
        audioVolumeManager.StopSFXAudio();

        // Show the "Lose Menu"
        loseMenu.SetActive(true);
        activeMenu = loseMenu;
    }

    public void HideMenu()
    {
        // Hide the active menu (if any)
        if (activeMenu != null)
        {
            // Save music and SFX slider values to PlayerPrefs
            PlayerPrefsManager.Save("MusicVolume", musicSlider.value);
            PlayerPrefsManager.Save("SFXVolume", sfxSlider.value);

            activeMenu.SetActive(false);
            activeMenu = null;
            // Resume the game by setting time scale back to 1
            Time.timeScale = 1f;

            // Stop the SFX audio
            audioVolumeManager.ResumeSFXAudio();
        }
    }

    public void ExitGame()
    {
        // Reset the time scale to 1 before loading the next scene
        Time.timeScale = 1f;

        // Stop the SFX audio
        audioVolumeManager.ResumeSFXAudio();

        // Save player data before exiting the game
        //playerData.Save();

        // Load the first scene (index 0 in the build settings)
        SceneManager.LoadScene(0);
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    // Function to save the player data
    //public void SavePlayerData()
    //{
        //playerData.Save();
    //}
}