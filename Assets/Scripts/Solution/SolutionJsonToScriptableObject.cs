using UnityEngine;
using UnityEditor;
using System.IO;
using System;

/// <summary>
/// Editor window class for converting a list of solutions from a JSON file into ScriptableObjects.
/// </summary>
public class SolutionJsonToScriptableObject : EditorWindow
{
    private string jsonFilePath = ".\\Assets\\Resources\\solutions.json"; // Default path to the JSON file

    /// <summary>
    /// Adds a menu item to the Unity Editor to show this editor window.
    /// </summary>
    [MenuItem("Tools/Convert Solutions JSON to ScriptableObject")]
    public static void ShowWindow()
    {
        // Opens the editor window with the title "Solution JSON Converter"
        GetWindow<SolutionJsonToScriptableObject>("Solution JSON Converter");
    }

    /// <summary>
    /// Method called to draw the GUI for the editor window.
    /// </summary>
    private void OnGUI()
    {
        // Display a label with bold style
        GUILayout.Label("Convert list of solutions from JSON to ScriptableObjects", EditorStyles.boldLabel);

        // Display a text field for the JSON file path
        jsonFilePath = EditorGUILayout.TextField("JSON File Path", jsonFilePath);

        // Display a button that triggers the conversion process
        if (GUILayout.Button("Convert"))
        {
            ConvertJsonToScriptableObject();
        }
    }

    /// <summary>
    /// Converts the solutions from the specified JSON file into ScriptableObjects.
    /// </summary>
    private void ConvertJsonToScriptableObject()
    {
        // Check if the specified JSON file exists
        if (File.Exists(jsonFilePath))
        {
            // Read the JSON data from the file
            string jsonData = File.ReadAllText(jsonFilePath);
            // Deserialize the JSON data into a SolutionList object
            SolutionList solutionList = JsonUtility.FromJson<SolutionList>(jsonData);

            // Iterate through each solution in the list
            foreach (var solution in solutionList.Solutions)
            {
                // Create a new ScriptableObject instance for each solution
                SolutionData solutionData = ScriptableObject.CreateInstance<SolutionData>();
                solutionData.puzzleId = solution.PuzzleId;
                solutionData.name = solution.Name;
                solutionData.sex = solution.Sex;

                // Define the asset path where the ScriptableObject will be saved
                string assetPath = $"Assets/ScriptableObjects/SolutionData/{solution.PuzzleId}.asset";
                // Create and save the ScriptableObject asset
                AssetDatabase.CreateAsset(solutionData, assetPath);
                AssetDatabase.SaveAssets();
            }

            // Refresh the AssetDatabase to ensure the new assets are recognized
            AssetDatabase.Refresh();
        }
        else
        {
            // Log an error if the JSON file is not found
            Debug.LogError("JSON file not found at the specified path.");
        }
    }
}
