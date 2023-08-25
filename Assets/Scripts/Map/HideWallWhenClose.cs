using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class HideWallWhenClose : MonoBehaviour
{
    Tilemap tr;
    Color OriginalColor;
    Color FadedCol;
    // Start is called before the first frame update
    void Start()
    {
        tr = GetComponent<Tilemap>();
        OriginalColor = tr.color;

        FadedCol = OriginalColor;
        FadedCol.a = 0.5f;
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        tr.color = FadedCol;
    }

    private void OnTriggerExit2D(Collider2D collision) {
        tr.color = OriginalColor;
    }
}
