using UnityEngine;
using UnityEditor;
using System.IO;
using System;

public class CharacterJsonToScriptableObject : EditorWindow
{
    private string jsonFilePath = ".\\Assets\\Resources\\characters.json";

    [MenuItem("Tools/Convert Character JSON to ScriptableObject")]
    public static void ShowWindow()
    {
        GetWindow<CharacterJsonToScriptableObject>("Character JSON Converter");
    }

    private void OnGUI()
    {
        GUILayout.Label("Convert list of characters from JSON to ScriptableObjects", EditorStyles.boldLabel);

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
            CharacterList characterList = JsonUtility.FromJson<CharacterList>(jsonData);

            foreach (var character in characterList.Characters)
            {
                CharacterData characterData = ScriptableObject.CreateInstance<CharacterData>();
                characterData.name = character.Name;
                characterData.surname = character.Surname;
                characterData.sex = character.Sex;
                characterData.image = character.Image;
                characterData.itemsOwned = ConvertItems(character.ItemsOwned);

                string assetPath = $"Assets/ScriptableObjects/CharacterData/{character.Name}_{character.Surname}_{character.Sex}.asset";
                AssetDatabase.CreateAsset(characterData, assetPath);
                AssetDatabase.SaveAssets();
            }

            AssetDatabase.Refresh();
        }
        else
        {
            Debug.LogError("JSON file not found at the specified path.");
        }
    }

    private ItemData[] ConvertItems(Item[] items)
    {
        ItemData[] itemDataArray = new ItemData[items.Length];
        for (int i = 0; i < items.Length; i++)
        {
            ItemData itemData = new ItemData
            {
                itemName = items[i].ItemName,
                price = Convert.ToInt32(items[i].Price)
            };
            itemDataArray[i] = itemData;
        }
        return itemDataArray;
    }
}
