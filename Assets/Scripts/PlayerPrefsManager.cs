using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class PlayerPrefsManager
{
    // Save a float value to PlayerPrefs with the given key
    public static void Save(string key, float value)
    {
        // Set the specified key to the given float value
        PlayerPrefs.SetFloat(key, value);
        
        // Save the changes made to PlayerPrefs
        PlayerPrefs.Save();
    }

    // Load a float value from PlayerPrefs using the given key
    public static float Load(string key)
    {
        // Retrieve and return the float value associated with the key
        return PlayerPrefs.GetFloat(key);
    }
}