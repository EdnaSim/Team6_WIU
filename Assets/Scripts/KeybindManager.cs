using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class KeybindManager : MonoBehaviour
{
    private Dictionary<string, KeyCode> keys = new Dictionary<string, KeyCode>();

    public TMP_Text up, down, left, right, sprint, attack, interact, pause, inventory, map;

    private GameObject currentKey;

    private Color32 normal = new Color32(39, 171, 249, 255);
    private Color32 selected = new Color32(239, 116, 36, 255);

    void Start()
    {
        keys.Add("Up", (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("Up", "W")));
        keys.Add("Down", (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("Down", "S")));
        keys.Add("Left", (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("Left", "A")));
        keys.Add("Right", (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("Right", "D")));
        keys.Add("Sprinting", (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("Sprinting", "LeftShift")));
        keys.Add("Attacking", (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("Attacking", "Mouse0")));
        keys.Add("Interacting", (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("Interacting", "F")));
        keys.Add("Pausing", (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("Pausing", "Escape")));
        keys.Add("Open Inventory", (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("Open Inventory", "B")));
        keys.Add("Open Map", (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("Open Map", "M")));

        up.text = keys["Up"].ToString();
        down.text = keys["Down"].ToString();
        left.text = keys["Left"].ToString();
        right.text = keys["Right"].ToString();
        sprint.text = keys["Sprinting"].ToString();
        attack.text = keys["Attacking"].ToString();
        interact.text = keys["Interacting"].ToString();
        pause.text = keys["Pausing"].ToString();
        inventory.text = keys["Open Inventory"].ToString();
        map.text = keys["Open Map"].ToString();
    }

    void Update()
    {
        if (Input.GetKeyDown(keys["Up"]))
        {
            Debug.Log("Move Up key pressed");
        }
        if (Input.GetKeyDown(keys["Down"]))
        {
            Debug.Log("Move Down key pressed");
        }
        if (Input.GetKeyDown(keys["Left"]))
        {
            Debug.Log("Move Left key pressed");
        }
        if (Input.GetKeyDown(keys["Right"]))
        {
            Debug.Log("Move Right key pressed");
        }
        if (Input.GetKeyDown(keys["Sprinting"]))
        {
            Debug.Log("Sprint key pressed");
        }
        if (Input.GetKeyDown(keys["Attacking"]))
        {
            Debug.Log("Attack key pressed");
        }
        if (Input.GetKeyDown(keys["Interacting"]))
        {
            Debug.Log("Interact key pressed");
        }
        if (Input.GetKeyDown(keys["Pausing"]))
        {
            Debug.Log("Pause key pressed");
        }
        if (Input.GetKeyDown(keys["Open Inventory"]))
        {
            Debug.Log("Inventory key pressed");
        }
        if (Input.GetKeyDown(keys["Open Map"]))
        {
            Debug.Log("Map key pressed");
        }
    }

    void OnGUI()
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

            currentKey.transform.GetChild(0).GetComponent<TMP_Text>().text = keys[currentKey.name].ToString();
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
}