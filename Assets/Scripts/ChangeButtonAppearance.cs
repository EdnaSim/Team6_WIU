using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class ChangeButtonAppearance : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerEnterHandler, IPointerExitHandler
{
    public Sprite newButtonImage;
    private Sprite originalButtonImage;
    public Button button;

    public TMP_Text buttonText;
    public float newSize = 60;
    private float originalSize;

    public int yOffset = 10;
    private Vector3 originalTextPosition;

    public Color highlightedColor;
    public Color pressedColor;
    private Color originalColor;

    private void Start()
    {
        originalButtonImage = button.image.sprite;

        originalSize = buttonText.fontSize;
        originalTextPosition = buttonText.transform.localPosition;
        originalColor = buttonText.color;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        buttonText.color = highlightedColor;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        buttonText.color = originalColor;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        button.image.sprite = newButtonImage;

        buttonText.fontSize = newSize;
        Vector3 newPos = originalTextPosition + new Vector3(0, yOffset, 0);
        buttonText.transform.localPosition = newPos;
        buttonText.color = pressedColor;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        button.image.sprite = originalButtonImage;

        buttonText.fontSize = originalSize;
        buttonText.transform.localPosition = originalTextPosition;
        buttonText.color = originalColor;
    }
}