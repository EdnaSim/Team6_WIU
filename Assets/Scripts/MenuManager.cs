using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class MenuManager : MonoBehaviour
{
    private GameObject activeMenu; // Reference to the currently active menu
    //public PlayerData playerData; // Reference to the PlayerData scriptable object

    public AudioVolumeManager audioVolumeManager;
    public Slider masterSlider; // Reference to the master slider in the UI
    public Slider musicSlider; // Reference to the music slider in the UI
    public Slider sfxSlider; // Reference to the SFX slider in the UI

    public GameObject winMenu; // Reference to the "Win Menu" GameObject
    public GameObject loseMenu; // Reference to the "Lose Menu" GameObject

    public GameObject loadingScreen; // Reference to the loading screen GameObject
    public Slider loadingSlider; // Reference to the loading slider in the loading screen
    public TMP_Text loadingPercentageText; // Reference to the text showing loading percentage
    private float targetProgress;

    private Slideshow slideshow;

    private void Start()
    {
        HideMenu(); // Ensure the menu is hidden when the scene starts
        loadingScreen.SetActive(false); // Hide the loading screen initially

        // Load player data when the game starts
        //playerData.Load();

        masterSlider.value = PlayerPrefsManager.Load("MasterVolume");
        // Load and set music and SFX slider values from PlayerPrefs
        musicSlider.value = PlayerPrefsManager.Load("MusicVolume");
        sfxSlider.value = PlayerPrefsManager.Load("SFXVolume");

        masterSlider.onValueChanged.AddListener(UpdateMasterVolume);
        // Add listeners to music and SFX sliders to update mixer volumes
        musicSlider.onValueChanged.AddListener(UpdateMusicVolume);
        sfxSlider.onValueChanged.AddListener(UpdateSFXVolume);
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

    public void PlayGame()
    {
        ShowLoadingScreen("SampleScene"); // Replace with the name of your game scene

        // Reset the time scale to 1 before loading the next scene
        Time.timeScale = 1f;
        //SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        //slideshow.OpenSlideshow();
    }

    // Function to reset the player data and s
    public void NewGame()
    {
        ShowLoadingScreen("SampleScene"); // Replace with the name of your game scene

        //playerData.Reset();
        // Reset the time scale to 1 before loading the next scene
        Time.timeScale = 1f;
        //SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    private void ShowLoadingScreen(string sceneName)
    {
        loadingSlider.value = 0f;
        targetProgress = 0f;

        StartCoroutine(LoadSceneWithLoadingScreen(sceneName));
    }

    IEnumerator LoadSceneWithLoadingScreen(string sceneName)
    {
        // Activate the loading screen
        loadingScreen.SetActive(true);

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
            PlayerPrefsManager.Save("MasterVolume", masterSlider.value);
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