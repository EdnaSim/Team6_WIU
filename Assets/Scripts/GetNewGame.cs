using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetNewGame : MonoBehaviour
{
    // A static variable to store whether it's a new game or not
    public static bool isNewGame;

    // Awake is called when the script instance is being loaded
    private void Awake()
    {
        // Initialize the isNewGame variable to false when the game starts
        isNewGame = false;
    }
}