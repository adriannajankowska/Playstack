using UnityEditor;
using UnityEngine;
using System.IO;

/// <summary>
/// Editor window for validating CharacterData ScriptableObjects.
/// </summary>
public class CharacterDataValidator : EditorWindow
{
    // Reference to the CharacterData asset selected by the user
    private CharacterData characterData;

    // Menu item to open the Character Data Validator window
    [MenuItem("Tools/Character Data Validator")]
    public static void ShowWindow()
    {
        // Create and show the window with the specified title
        GetWindow<CharacterDataValidator>("Character Data Validator");
    }

    /// <summary>
    /// Draws the GUI elements for the Editor window.
    /// </summary>
    private void OnGUI()
    {
        // Display the label for the validator
        GUILayout.Label("Validate data of a selected Character", EditorStyles.boldLabel);

        // Input field for selecting a CharacterData asset from the project file hierarchy
        characterData = (CharacterData)EditorGUILayout.ObjectField("Character Data", characterData, typeof(CharacterData), false);

        // Button to start the validation process
        if (characterData != null && GUILayout.Button("Validate"))
        {
            // Validate the selected CharacterData
            ValidateCharacterData();
        }
    }

    /// <summary>
    /// Validates all CharacterData assets selected for the validation.
    /// </summary>
    private void ValidateCharacterData()
    {
        // Find all CharacterData assets in the project
        string[] guids = AssetDatabase.FindAssets("t:CharacterData");

        // Iterate through each GUID and validate the corresponding CharacterData
        foreach (string guid in guids)
        {
            // Load the CharacterData asset at the specified path
            string path = AssetDatabase.GUIDToAssetPath(guid);
            CharacterData characterData = AssetDatabase.LoadAssetAtPath<CharacterData>(path);

            // Check for errors in the CharacterData fields
            ValidateCharacterDataFields(characterData);
        }
    }

    /// <summary>
    /// Validates the fields of a given CharacterData asset.
    /// </summary>
    /// <param name="characterData">The CharacterData asset to validate.</param>
    private void ValidateCharacterDataFields(CharacterData characterData)
    {
        bool hasError = false; // Flag to track if any errors were found
        string errorMessage = $"Errors in the Character validation:\n"; // Error message string

        // Check if the name field is empty
        if (string.IsNullOrEmpty(characterData.name))
        {
            hasError = true;
            errorMessage += "- Name is missing\n";
        }

        // Check if the surname field is empty
        if (string.IsNullOrEmpty(characterData.surname))
        {
            hasError = true;
            errorMessage += "- Surname is missing\n";
        }

        // Check if the sex field is empty
        if (string.IsNullOrEmpty(characterData.sex))
        {
            hasError = true;
            errorMessage += "- Sex is missing\n";
        }

        // Check if the image field is empty or the path is invalid
        if (string.IsNullOrEmpty(characterData.image))
        {
            hasError = true;
            errorMessage += "- Image is missing\n";
        }
        else if (!File.Exists(characterData.image) || !IsImageFile(characterData.image))
        {
            hasError = true;
            errorMessage += "- Image path is invalid or file is not an image\n";
        }

        // Check if the itemsOwned array is empty
        if (characterData.itemsOwned == null)
        {
            hasError = true;
            errorMessage += "- ItemsOwned is empty\n";
        }
        else
        {
            // Iterate through each item in the itemsOwned array
            foreach (ItemData item in characterData.itemsOwned)
            {
                if (item == null)
                {
                    hasError = true;
                    errorMessage += "- ItemData is missing\n";
                }
                else
                {
                    // Check if the itemName field is empty
                    if (string.IsNullOrEmpty(item.itemName))
                    {
                        hasError = true;
                        errorMessage += "- ItemData has missing itemName\n";
                    }
                    // Check if the price field is empty or zero
                    if (item.price == 0)
                    {
                        hasError = true;
                        errorMessage += "- ItemData has missing or zero price\n";
                    }
                }
            }
        }

        // Update the validation log field with the error message or success message
        characterData.validationLog = hasError ? errorMessage : "No errors found, all good :)";
    }

    /// <summary>
    /// Checks if the given file path points to a valid image file.
    /// </summary>
    /// <paramname="path">The file path to check.</param>
    /// <returns>True if the file is a valid image, false otherwise.</returns>
    private bool IsImageFile(string path)
    {
        // Array of valid image file extensions
        string[] validExtensions = { ".png", ".jpg", ".jpeg", ".gif", ".bmp" };

        // Get the file extension and convert it to lowercase
        string extension = Path.GetExtension(path).ToLower();

        // Check if the extension is in the list of valid extensions
        return System.Array.Exists(validExtensions, ext => ext == extension);
    }
}
