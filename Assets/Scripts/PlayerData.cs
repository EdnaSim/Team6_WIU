using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu]
public class PlayerData : ScriptableObject
{
    private void OnEnable()
    {
        // called when the instance is setup
    }

    public void Load()
    {
        string s;
        if (FileManager.LoadFromFile("playerdata.json", out s))
        {
            JsonUtility.FromJsonOverwrite(s, this);
        }
    }

    public void Save()
    {
        string s = JsonUtility.ToJson(this);
        if(FileManager.WriteToFile("playerdata.json", s))
        {
            Debug.Log("Save was successful!");
        }
    }

    private void OnValidate()
    {
        // called when any value is changed in the inspector
    }

    public void Reset()
    {
        // Save the updated playerData after resetting the values
        Save();
    }
}