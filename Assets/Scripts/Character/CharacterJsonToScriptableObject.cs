using UnityEngine;
using UnityEditor;
using System.IO;
using System;

/// <summary>
/// A Unity Editor window for converting JSON character data to ScriptableObjects.
/// </summary>
public class CharacterJsonToScriptableObject : EditorWindow
{
    // Path to the JSON file containing character data
    private string jsonFilePath = ".\\Assets\\Resources\\characters.json";

    // Menu item to open the Character JSON Converter window
    [MenuItem("Tools/Convert Character JSON to ScriptableObject")]
    public static void ShowWindow()
    {
        // Create and show the window with the specified title
        GetWindow<CharacterJsonToScriptableObject>("Character JSON Converter");
    }

    /// <summary>
    /// Draws the GUI elements for the Editor window.
    /// </summary>
    private void OnGUI()
    {
        // Display the label for the converter
        GUILayout.Label("Convert list of characters from JSON to ScriptableObjects", EditorStyles.boldLabel);

        // Input field for the JSON file path
        jsonFilePath = EditorGUILayout.TextField("JSON File Path", jsonFilePath);

        // Button to start the conversion process
        if (GUILayout.Button("Convert"))
        {
            try
            {
                // Attempt to convert the JSON data to ScriptableObjects
                ConvertJsonToScriptableObject();
            }
            catch (Exception ex)
            {
                // Log any exceptions that occur during the conversion
                Debug.LogError($"Conversion failed: {ex.Message}");
            }
        }
    }

    /// <summary>
    /// Converts the JSON data from the specified file to ScriptableObjects.
    /// </summary>
    private void ConvertJsonToScriptableObject()
    {
        // Check if the JSON file exists at the specified path
        if (File.Exists(jsonFilePath))
        {
            // Read the JSON data from the file
            string jsonData = File.ReadAllText(jsonFilePath);

            // Deserialize the JSON data into a CharacterList object
            CharacterList characterList = JsonUtility.FromJson<CharacterList>(jsonData);

            // Iterate through each character in the list
            foreach (var character in characterList.Characters)
            {
                // Create a new CharacterData ScriptableObject
                CharacterData characterData = ScriptableObject.CreateInstance<CharacterData>();
                characterData.name = character.Name;
                characterData.surname = character.Surname;
                characterData.sex = character.Sex;
                characterData.image = character.Image;
                characterData.itemsOwned = ConvertItems(character.ItemsOwned);

                // Define the asset path for the new ScriptableObject
                string assetPath = $"Assets/ScriptableObjects/CharacterData/{character.Name}_{character.Surname}_{character.Sex}.asset";

                // Create and save the new ScriptableObject asset
                AssetDatabase.CreateAsset(characterData, assetPath);
                AssetDatabase.SaveAssets();
            }

            // Refresh the AssetDatabase to show the new assets
            AssetDatabase.Refresh();
        }
        else
        {
            // Log an error if the JSON file was not found
            Debug.LogError("JSON file not found at the specified path.");
        }
    }

    /// <summary>
    /// Converts an array of Item objects to an array of ItemData objects.
    /// </summary>
    /// <paramname="items">The array of Character's Item objects to be aaded to the Character.</param>
    /// <returns>An array of converted ItemData objects.</returns>
    private ItemData[] ConvertItems(Item[] items)
    {
        // Initialize a new array of ItemData with the same length as the input array
        ItemData[] itemDataArray = new ItemData[items.Length];

        // Iterate through each item and convert it to an ItemData object
        for (int i = 0; i < items.Length; i++)
        {
            itemDataArray[i] = new ItemData
            {
                itemName = items[i].ItemName,
                price = items[i].Price
            };
        }

        // Return the array of converted ItemData objects
        return itemDataArray;
    }
}
