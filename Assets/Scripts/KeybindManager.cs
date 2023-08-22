using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class KeybindManager : MonoBehaviour
{
    private Dictionary<string, KeyCode> keys = new Dictionary<string, KeyCode>();
    private Dictionary<string, KeyCode> defaultKeys = new Dictionary<string, KeyCode>();

    public List<Button> keyButtons; // Store all the buttons for keybinds
    public Button saveKeysButton;

    private GameObject currentKey;

    private Color32 normal = new Color32(39, 171, 249, 255);
    private Color32 selected = new Color32(239, 116, 36, 255);

    private void Start()
    {
        LoadDefaultKeys();
        LoadKeys();
    }

    private void LoadDefaultKeys()
    {
        foreach (var keyButton in keyButtons)
        {
            defaultKeys.Add(keyButton.name, GetDefaultKey(keyButton.name));
        }
    }

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
            case "Pausing":
                return KeyCode.Escape;
            case "Open Inventory":
                return KeyCode.B;
            case "Open Map":
                return KeyCode.M;
            default:
                return KeyCode.None; // Return None as default for unknown key names
        }
    }

    private void LoadKeys()
    {
        foreach (var keyButton in keyButtons)
        {
            KeyCode loadedKey = (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString(keyButton.name, defaultKeys[keyButton.name].ToString()));
            keys.Add(keyButton.name, loadedKey);
            keyButton.GetComponentInChildren<TMP_Text>().text = GetKeyDisplayName(loadedKey);
        }
        UpdateSaveKeysButtonInteractable();
    }

    private string GetKeyDisplayName(KeyCode key)
    {
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
        foreach (var key in keys)
        {
            if (Input.GetKeyDown(key.Value))
            {
                Debug.Log(key.Key + " key pressed");
            }
        }
        UpdateSaveKeysButtonInteractable();
    }

    public void ShowKeyBindMenu()
    {
        foreach (var keyButton in keyButtons)
        {
            KeyCode loadedKey = (KeyCode)System.Enum.Parse(typeof(KeyCode), keyButton.GetComponentInChildren<TMP_Text>().text = PlayerPrefs.GetString(keyButton.name));
            keyButton.GetComponentInChildren<TMP_Text>().text = GetKeyDisplayName(loadedKey);
        }
    }

    private void OnGUI()
    {
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
        if (currentKey != null)
        {
            currentKey.GetComponent<Image>().color = normal;
        }

        currentKey = clicked;
        currentKey.GetComponent<Image>().color = selected;
    }

    public void SaveKeys()
    {
        foreach (var key in keys)
        {
            PlayerPrefs.SetString(key.Key, key.Value.ToString());
        }

        PlayerPrefs.Save();
    }

    private void UpdateSaveKeysButtonInteractable()
    {
        HashSet<KeyCode> usedKeyCodes = new HashSet<KeyCode>();

        foreach (var key in keys)
        {
            if (usedKeyCodes.Contains(key.Value))
            {
                saveKeysButton.interactable = false;
                return;
            }
            usedKeyCodes.Add(key.Value);
        }

        saveKeysButton.interactable = true;
    }
}