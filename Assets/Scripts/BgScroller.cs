using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BgScroller : MonoBehaviour
{
    [SerializeField]
    private RawImage image;
    [SerializeField]
    private float scrollSpeed = 0.01f; // The speed of scrolling
    private Vector2 textureOffset = Vector2.zero; // Store the texture offset

    // Update is called once per frame
    void Update()
    {
        // Update the texture offset based on the scroll speed
        textureOffset.x += scrollSpeed * Time.deltaTime; // Scroll in X
        textureOffset.y += scrollSpeed * Time.deltaTime; // Scroll in Y

        // Apply the updated texture offset to the RawImage
        image.uvRect = new Rect(textureOffset.x, textureOffset.y, 1, 1);

        // Check if the texture has scrolled completely to the left (off-screen)
        if (textureOffset.x >= 1.0f)
        {
            // If it has, reset the X texture offset to start the loop again
            textureOffset.x = 0.0f;
        }

        // Check if the texture has scrolled completely upwards (off-screen)
        if (textureOffset.y >= 1.0f)
        {
            // If it has, reset the Y texture offset to start the loop again
            textureOffset.y = 0.0f;
        }
    }
}