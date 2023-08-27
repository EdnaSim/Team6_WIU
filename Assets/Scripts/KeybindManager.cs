using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class KeybindManager : MonoBehaviour
{
    // Singleton instance
    public static KeybindManager Instance;

    // Dictionaries to store current and default key bindings
    private Dictionary<string, KeyCode> keys = new Dictionary<string, KeyCode>();
    private Dictionary<string, KeyCode> defaultKeys = new Dictionary<string, KeyCode>();

    // UI elements
    public List<Button> keyButtons; // Store all the buttons for keybinds
    public Button saveKeysButton;
    private GameObject currentKey;

    // Colors for button highlighting
    private Color32 normal = new Color32(39, 171, 249, 255);
    private Color32 selected = new Color32(239, 116, 36, 255);

    private void Awake()
    {
        Instance = this; // Assign the singleton instance
        //PlayerPrefs.DeleteAll();
        // Load default and saved key bindings
        LoadDefaultKeys();

        foreach (var keyButton in keyButtons)
        {
            if (System.Enum.IsDefined(typeof(KeyCode), PlayerPrefs.GetString(keyButton.name)) == false)
            {
                PlayerPrefs.SetString(keyButton.name, defaultKeys[keyButton.name].ToString());
                string keyText = PlayerPrefs.GetString(keyButton.name);
                
                if (System.Enum.TryParse(keyText, out KeyCode parsedKeyCode))
                {
                    // Use the loadedKey variable
                    KeyCode loadedKey = parsedKeyCode;
                }
                else
                {
                    Debug.Log("Failed to parse KeyCode: " + keyText);
                }
            }
        }   
        LoadKeys();
    }

    // Load default keys for all buttons
    private void LoadDefaultKeys()
    {
        // Initialize default key bindings
        foreach (var keyButton in keyButtons)
        {
            defaultKeys.Add(keyButton.name, GetDefaultKey(keyButton.name));
        }
    }

    // Get the default key for a given key name
    private KeyCode GetDefaultKey(string keyName)
    {
        // Assign default keys based on the key name
        switch (keyName)
        {
            case "Up":
                return KeyCode.W;
            case "Down":
                return KeyCode.S;
            case "Left":
                return KeyCode.A;
            case "Right":
                return KeyCode.D;
            case "Sprinting":
                return KeyCode.LeftShift;
            case "Attacking":
                return KeyCode.Mouse0;
            case "Interacting":
                return KeyCode.F;
            default:
                return KeyCode.None; // Return None as default for unknown key names
        }
    }

    // Load saved keys or use default keys for all buttons
    private void LoadKeys()
    {
        // Load saved key bindings and update UI
        foreach (var keyButton in keyButtons)
        {
            KeyCode loadedKey = (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString(keyButton.name, defaultKeys[keyButton.name].ToString()));
            keys.Add(keyButton.name, loadedKey);
            keyButton.GetComponentInChildren<TMP_Text>().text = GetKeyDisplayName(loadedKey);
        }
        UpdateSaveKeysButtonInteractable();
    }

    // Get a display name for a given KeyCode
    private string GetKeyDisplayName(KeyCode key)
    {
        // Change key name "Mouse0" & "Mouse1" to display "LMB" & "RMB" respectively
        if (key == KeyCode.Mouse0)
        {
            return "LMB";
        }
        else if (key == KeyCode.Mouse1)
        {
            return "RMB";
        }
        else
        {
            return key.ToString();
        }
    }

    private void Update()
    {
        // Check for key presses and update the UI accordingly
        foreach (var key in keys)
        {
            if (Input.GetKeyDown(key.Value))
            {
                //Debug.Log(key.Key + " key pressed");
            }
        }
        UpdateSaveKeysButtonInteractable();
    }

    public void ShowKeyBindMenu()
    {
        // Show the current keybinds in the UI
        foreach (var keyButton in keyButtons)
        {
            KeyCode loadedKey = (KeyCode)System.Enum.Parse(typeof(KeyCode), keyButton.GetComponentInChildren<TMP_Text>().text = PlayerPrefs.GetString(keyButton.name));
            keyButton.GetComponentInChildren<TMP_Text>().text = GetKeyDisplayName(loadedKey);
        }
    }

    private void OnGUI()
    {
        // Listen for key or mouse button presses when changing keybindings
        if (currentKey != null)
        {
            Event e = Event.current;
            if (e.isKey || e.isMouse)
            {
                if (e.isKey)
                {
                    keys[currentKey.name] = e.keyCode;
                }
                else if (e.isMouse)
                {
                    int mouseButtonIndex = (int)e.button;
                    keys[currentKey.name] = KeyCode.Mouse0 + mouseButtonIndex;
                }

                currentKey.GetComponentInChildren<TMP_Text>().text = GetKeyDisplayName(keys[currentKey.name]);
                currentKey.GetComponent<Image>().color = normal;
                currentKey = null;
            }
        }
    }

    public void ChangeKey(GameObject clicked)
    {
        // Change the keybinding for a button
        if (currentKey != null)
        {
            currentKey.GetComponent<Image>().color = normal;
        }

        currentKey = clicked;
        currentKey.GetComponent<Image>().color = selected;
    }

    // Update the state of the "Save Keys" button based on key changes
    private void UpdateSaveKeysButtonInteractable()
    {
        // Validate key configuration and enable/disable save button
        HashSet<KeyCode> usedKeyCodes = new HashSet<KeyCode>();
        Dictionary<KeyCode, List<string>> sameKeys = new Dictionary<KeyCode, List<string>>();

        bool hasChangedKey = false; // Flag to track if any key has changed
        foreach (var key in keys)
        {
            if (usedKeyCodes.Contains(key.Value))
            {
                if (sameKeys.ContainsKey(key.Value))
                {
                    sameKeys[key.Value].Add(key.Key);
                }
                else
                {
                    sameKeys[key.Value] = new List<string> { key.Key };
                }
            }
            else
            {
                usedKeyCodes.Add(key.Value);
            }

            string previousValue = PlayerPrefs.GetString(key.Key, "");
            string newValue = key.Value.ToString();

            if (previousValue != newValue)
            {
                hasChangedKey = true;
            }
        }

        // Check if the current key configuration is valid
        bool isValidConfiguration = hasChangedKey && sameKeys.Count == 0;
        if (isValidConfiguration)
        {
            saveKeysButton.interactable = true;
        }
        else
        {
            saveKeysButton.interactable = false;
        }

        if (sameKeys.Count > 0)
        {
            foreach (var pair in sameKeys)
            {
                Debug.Log("Keys " + string.Join(", ", pair.Value) + " have the same value: " + pair.Key);
            }
            //Debug.Log("The configuration is not valid.");
        }
        else if (hasChangedKey)
        {
            //Debug.Log("The configuration is valid.");
        }
        else
        {
            //Debug.Log("No keys were changed, so nothing to validate.");
        }
    }

    // Save changed key bindings to PlayerPrefs
    public void SaveKeys()
    {
        bool keysChanged = false; // Flag to track if any keys were changed
        foreach (var key in keys)
        {
            string previousValue = PlayerPrefs.GetString(key.Key, "");
            string newValue = key.Value.ToString();

            if (previousValue != newValue)
            {
                PlayerPrefs.SetString(key.Key, newValue);
                keysChanged = true;

                Debug.Log(key.Key + " value changed: Previous Value - " + previousValue + ", New Value - " + newValue);
            }
        }

        if (keysChanged)
        {
            PlayerPrefs.Save();
            Debug.Log("Keys saved successfully.");
        }
        else
        {
            //Debug.Log("No keys were changed, so nothing was saved.");
        }
        UpdateSaveKeysButtonInteractable();
    }

    public KeyCode GetKeyForAction(string actionName)
    {
        if (keys.TryGetValue(actionName, out KeyCode key))
        {
            return key;
        }
        return KeyCode.None; // Return None if the action name is not found
    }
}