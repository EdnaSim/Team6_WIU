using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class ChangeButtonAppearance : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerEnterHandler, IPointerExitHandler
{
    // Reference to the new button image sprite
    public Sprite newButtonImage;
    // Stores the original button image sprite
    private Sprite originalButtonImage;
    // Reference to the Button component
    public Button button;

    // Reference to the TextMeshPro text component
    public TMP_Text buttonText;
    // New font size to apply
    public float newSize = 60;
    // Stores the original font size
    private float originalSize;

    // Vertical offset for the text position
    public int yOffset = 10;
    // Stores the original text position
    private Vector3 originalTextPosition;

    // Highlighted color for the text
    public Color highlightedColor;
    // Pressed color for the text
    public Color pressedColor;
    // Stores the original text color
    private Color originalColor;

    private void Start()
    {
        // Store the original button image sprite
        originalButtonImage = button.image.sprite;

        // Store the original font size, text position, and color
        originalSize = buttonText.fontSize;
        originalTextPosition = buttonText.transform.localPosition;
        originalColor = buttonText.color;
    }

    // Called when the pointer enters the button's area
    public void OnPointerEnter(PointerEventData eventData)
    {
        // Change the text color to the highlighted color
        buttonText.color = highlightedColor;
    }

    // Called when the pointer exits the button's area
    public void OnPointerExit(PointerEventData eventData)
    {
        // Restore the original text color
        buttonText.color = originalColor;
    }

    // Called when the button is pressed
    public void OnPointerDown(PointerEventData eventData)
    {
        // Change the button image sprite
        button.image.sprite = newButtonImage;

        // Change the font size and position of the text
        buttonText.fontSize = newSize;
        Vector3 newPos = originalTextPosition + new Vector3(0, yOffset, 0);
        buttonText.transform.localPosition = newPos;

        // Change the text color to the pressed color
        buttonText.color = pressedColor;
    }

    // Called when the button is released
    public void OnPointerUp(PointerEventData eventData)
    {
        // Restore the original button image sprite
        button.image.sprite = originalButtonImage;

        // Restore the original font size and text position
        buttonText.fontSize = originalSize;
        buttonText.transform.localPosition = originalTextPosition;

        // Restore the original text color
        buttonText.color = originalColor;
    }
}