using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Slideshow : MonoBehaviour
{
    public GameObject[] slides; // An array of slide GameObjects
    private int currentSlideIndex = 0; // Index of the currently displayed slide
    private bool isSlideshowActive = false; // Whether the slideshow is currently active

    private void Start()
    {
        // Deactivate all slides when the slideshow starts
        foreach (GameObject slide in slides)
        {
            slide.SetActive(false);
        }
    }

    private void Update()
    {
        // Check if the slideshow is active or if it's a new game
        if (isSlideshowActive == true || GetNewGame.isNewGame)
        {
            // Show the current slide and hide others
            for (int i = 0; i < slides.Length; i++)
            {
                slides[i].SetActive(i == currentSlideIndex);
            }

            // Check for arrow key input
            if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                ShowPreviousSlide();
            }
            else if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                ShowNextSlide();
            }

            // Check for Esc key input to exit slideshow
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                ExitSlideshow();
            }
        }
    }

    public void ActivateSlideshow()
    {
        isSlideshowActive = true;
    }

    public void ShowNextSlide()
    {
        // Move to the next slide if possible
        if (currentSlideIndex < slides.Length - 1)
        {
            currentSlideIndex++;
        }
    }

    public void ShowPreviousSlide()
    {
        // Move to the previous slide if possible
        if (currentSlideIndex > 0)
        {
            currentSlideIndex--;
        }
    }

    public void ExitSlideshow()
    {
        // Destroy all slide GameObjects and clear the slides array
        foreach (GameObject slide in slides)
        {
            Destroy(slide);
        }
        slides = new GameObject[0]; // Clear the slides array
        currentSlideIndex = 0;
    }
}