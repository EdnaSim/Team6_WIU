using System;
using System.IO;
using System.Threading.Tasks;
using UnityEngine;

public static class FileManager
{
    // Function to write data to a file
    public static bool WriteToFile(string a_FileName, string a_FileContents)
    {
        // Get the full path to the file in the persistent data path
        var fullPath = Path.Combine(Application.persistentDataPath, a_FileName);
        try
        {
            // Write the contents to the file
            File.WriteAllText(fullPath, a_FileContents);
            return true; // Return true if writing was successful
        }
        catch (Exception e)
        {
            // Log an error message if writing fails and return false
            Debug.LogError($"Failed to write to {fullPath} with exception {e}");
            return false;
        }
    }

    // Function to load data from a file
    public static bool LoadFromFile(string a_FileName, out string result)
    {
        // Get the full path to the file in the persistent data path
        var fullPath = Path.Combine(Application.persistentDataPath, a_FileName);
        try
        {
            // Read the contents from the file
            result = File.ReadAllText(fullPath);
            return true; // Return true if reading was successful
        }
        catch (Exception e)
        {
            // Log an error message if reading fails, set result to empty, and return false
            Debug.LogError($"Failed to read from {fullPath} with exception {e}");
            result = "";
            return false;
        }
    }
}