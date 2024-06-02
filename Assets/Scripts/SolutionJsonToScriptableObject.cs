using UnityEngine;
using UnityEditor;
using System.IO;
using System;

public class SolutionJsonToScriptableObject : EditorWindow
{
    private string jsonFilePath = ".\\Assets\\Resources\\solutions.json";

    [MenuItem("Tools/Convert Solutions JSON to ScriptableObject")]
    public static void ShowWindow()
    {
        GetWindow<SolutionJsonToScriptableObject>("Solution JSON Converter");
    }

    private void OnGUI()
    {
        GUILayout.Label("Convert list of solutions from JSON to ScriptableObjects", EditorStyles.boldLabel);

        jsonFilePath = EditorGUILayout.TextField("JSON File Path", jsonFilePath);

        if (GUILayout.Button("Convert"))
        {
            ConvertJsonToScriptableObject();
        }
    }

    private void ConvertJsonToScriptableObject()
    {
        if (File.Exists(jsonFilePath))
        {
            string jsonData = File.ReadAllText(jsonFilePath);
            SolutionList solutionList = JsonUtility.FromJson<SolutionList>(jsonData);

            foreach (var solution in solutionList.Solutions)
            {
                SolutionData solutionData = ScriptableObject.CreateInstance<SolutionData>();
                solutionData.puzzleId = solution.PuzzleId; 
                solutionData.name = solution.Name;
                solutionData.sex = solution.Sex;

                string assetPath = $"Assets/ScriptableObjects/SolutionData/{solution.PuzzleId}.asset";
                AssetDatabase.CreateAsset(solutionData, assetPath);
                AssetDatabase.SaveAssets();
            }

            AssetDatabase.Refresh();
        }
        else
        {
            Debug.LogError("JSON file not found at the specified path.");
        }
    }
}
