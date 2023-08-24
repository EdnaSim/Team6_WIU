using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class MenuManager : MonoBehaviour
{
    private GameObject activeMenu; // Reference to the currently active menu
    private Slideshow slideshow; // Reference to the Slideshow component

    public GameObject winMenu; // Reference to the "Win Menu" GameObject
    public GameObject loseMenu; // Reference to the "Lose Menu" GameObject

    public GameObject loadingScreen; // Reference to the loading screen GameObject
    private Slider loadingSlider; // Reference to the loading slider in the loading screen
    private TMP_Text loadingPercentageText; // Reference to the text showing loading percentage
    private float targetProgress; // The target progress for loading

    public AudioVolumeManager audioVolumeManager; // Reference to the AudioVolumeManager script
    public Slider masterSlider; // Reference to the master slider in the UI
    public Slider musicSlider; // Reference to the music slider in the UI
    public Slider sfxSlider; // Reference to the SFX slider in the UI

    private void Start()
    {
        HideMenu(); // Ensure the menu is hidden when the scene starts

        // Find the Slideshow component in the scene
        slideshow = FindObjectOfType<Slideshow>();

        if (loadingScreen != null)
        {
            // Get the TMP_Text and Slider components as children of loadingScreen
            loadingPercentageText = loadingScreen.GetComponentInChildren<TMP_Text>();
            loadingSlider = loadingScreen.GetComponentInChildren<Slider>();
            loadingScreen.SetActive(false); // Hide the loading screen initially
        }

        // Load and set master, music and SFX slider values from PlayerPrefs
        masterSlider.value = PlayerPrefsManager.Load("MasterVolume");
        musicSlider.value = PlayerPrefsManager.Load("MusicVolume");
        sfxSlider.value = PlayerPrefsManager.Load("SFXVolume");

        // Add listeners to master, music and SFX sliders to update mixer volumes
        masterSlider.onValueChanged.AddListener(UpdateMasterVolume);
        musicSlider.onValueChanged.AddListener(UpdateMusicVolume);
        sfxSlider.onValueChanged.AddListener(UpdateSFXVolume);

        // Load player data when the game starts
        //playerData.Load();
    }

    private void ShowLoadingScreen(string sceneName)
    {
        if (loadingScreen != null)
        {
            loadingSlider.value = 0f;
            targetProgress = 0f;
            loadingScreen.SetActive(true); // Show the loading screen
        }
        StartCoroutine(LoadSceneWithLoadingScreen(sceneName));
    }

    IEnumerator LoadSceneWithLoadingScreen(string sceneName)
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneName);
        operation.allowSceneActivation = false; // Prevent the scene from activating automatically

        while (!operation.isDone)
        {
            // Update the target progress gradually based on FPS
            targetProgress = Mathf.Clamp01(operation.progress / 0.9f);

            // Update the loading slider and percentage text
            loadingSlider.value = targetProgress;
            loadingPercentageText.text = string.Format("Loading: {0}%", (int)(targetProgress * 100));

            // If the target progress reaches 1, activate the scene manually
            if (targetProgress >= 1f)
            {
                operation.allowSceneActivation = true;
            }
            yield return null;
        }
    }

    private void UpdateMasterVolume(float volume)
    {
        audioVolumeManager.SetMasterVolume(volume);
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

    // Function to start a new game
    public void NewGame()
    {
        // Reset the time scale to 1 before loading the next scene
        Time.timeScale = 1f;
        ShowLoadingScreen("SampleScene");
        //playerData.Reset();
        GetNewGame.isNewGame = true;

        if (slideshow != null)
        {
            // Find the Slideshow component in the scene and activate it
            slideshow.ActivateSlideshow();
        }
    }

    // Function to play the game
    public void PlayGame()
    {
        // Reset the time scale to 1 before loading the next scene
        Time.timeScale = 1f;
        ShowLoadingScreen("SampleScene");
    }

    // Function to show a specific menu
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

    // Function to hide the active menu
    public void HideMenu()
    {
        // Hide the active menu (if any)
        if (activeMenu != null)
        {
            // Save master, music and SFX slider values to PlayerPrefs
            PlayerPrefsManager.Save("MasterVolume", masterSlider.value);
            PlayerPrefsManager.Save("MusicVolume", musicSlider.value);
            PlayerPrefsManager.Save("SFXVolume", sfxSlider.value);

            // Resume the game by setting time scale back to 1
            Time.timeScale = 1f;

            // Resume the SFX audio
            audioVolumeManager.ResumeSFXAudio();

            // Deactivate the active menu
            activeMenu.SetActive(false);
            activeMenu = null;
        }
    }

    // Function to show the "Win Menu"
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

    // Function to show the "Lose Menu"
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

    // Function to exit the game and go back to the main menu
    public void ExitGame()
    {
        // Reset the time scale to 1 before loading the main menu scene
        Time.timeScale = 1f;

        // Resume the SFX audio
        audioVolumeManager.ResumeSFXAudio();

        // Load the first scene (index 0 in the build settings), which is the main menu
        SceneManager.LoadScene(0);
    }

    // Function to quit the application
    public void QuitGame()
    {
        Application.Quit();
    }
}