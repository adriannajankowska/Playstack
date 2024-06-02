using UnityEditor;
using UnityEngine;
using System.IO;

public class CharacterDataValidator : EditorWindow
{
    private CharacterData characterData;

    [MenuItem("Tools/Character Data Validator")]
    public static void ShowWindow()
    {
        GetWindow<CharacterDataValidator>("Character Data Validator");
    }

    private void OnGUI()
    {
        GUILayout.Label("Validate data of a selected Character", EditorStyles.boldLabel);

        characterData = (CharacterData)EditorGUILayout.ObjectField("Character Data", characterData, typeof(CharacterData), false);

        if (characterData != null && GUILayout.Button("Validate"))
        {
            ValidateCharacterData();
        }
    }

    private void ValidateCharacterData()
    {
        // Find all CharacterData assets in the project
        string[] guids = AssetDatabase.FindAssets("t:CharacterData");
        foreach (string guid in guids)
        {
            //string path = AssetDatabase.GUIDToAssetPath(guid);
            //CharacterData characterData = AssetDatabase.LoadAssetAtPath<CharacterData>(path);

            // Check for errors
            ValidateCharacterDataFields(characterData);
        }
    }

    private void ValidateCharacterDataFields(CharacterData characterData)
    {
        bool hasError = false;
        string errorMessage = $"Errors in the Character validation:\n";

        // Check name
        if (string.IsNullOrEmpty(characterData.name))
        {
            hasError = true;
            errorMessage += "- Name is missing\n";
        }

        // Check surname
        if (string.IsNullOrEmpty(characterData.surname))
        {
            hasError = true;
            errorMessage += "- Surname is missing\n";
        }

        // Check sex
        if (string.IsNullOrEmpty(characterData.sex))
        {
            hasError = true;
            errorMessage += "- Sex is missing\n";
        }

        // Check image
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

        // Check itemsOwned
        if (characterData.itemsOwned == null)
        {
            hasError = true;
            errorMessage += "- ItemsOwned is empty\n";
        }
        else
        {
            foreach (ItemData item in characterData.itemsOwned)
            {
                if (item == null)
                {
                    hasError = true;
                    errorMessage += "- ItemData is missing\n";
                }
                else
                {
                    // Check itemName in itemsOwned
                    if (string.IsNullOrEmpty(item.itemName))
                    {
                        hasError = true;
                        errorMessage += "- ItemData has missing itemName\n";
                    }
                    // Check price in itemsOwned
                    if (string.IsNullOrEmpty(item.price.ToString()) || item.price == 0)
                    {
                        hasError = true;
                        errorMessage += "- ItemData has missing price\n";
                    }
                }
            }
        }

        // Update the validation log field
        characterData.validationLog = hasError ? errorMessage : "No errors found, all good :)";
    }

    private bool IsImageFile(string path)
    {
        string[] validExtensions = { ".png", ".jpg", ".jpeg", ".gif", ".bmp" };
        string extension = Path.GetExtension(path).ToLower();
        return System.Array.Exists(validExtensions, ext => ext == extension);
    }
}
