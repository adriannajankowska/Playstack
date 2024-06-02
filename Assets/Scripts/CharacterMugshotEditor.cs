using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(CharacterData))]
public class CharacterMugshotEditor : Editor
{
    private InventoryManager inventoryManager;
    private string inventoryTag = "CharacterInventory"; // Set the default tag for character inventory

    private void OnEnable()
    {
        inventoryManager = new InventoryManager();
        inventoryManager.Initialize(inventoryTag);
    }

    public override void OnInspectorGUI()
    {
        // Reference to the target object being inspected
        CharacterData characterData = (CharacterData)target;

        // Display the default inspector fields for the CharacterData component
        DrawDefaultInspector();

        GUILayout.Space(10);
        GUILayout.Label("Create a Mugshot of a Character from its ScriptableObject", EditorStyles.boldLabel);

        if (GUILayout.Button("Create Character Mugshot"))
        {
            MugshotCreator.CreateCharacterMugshot(characterData, inventoryManager);
            
        }

        // Repaint the editor to avoid layout issues
        if (Event.current.type == EventType.Repaint)
        {
            EditorGUIUtility.labelWidth = 0; // Reset label width
            //EditorWindow.focusedWindow.Repaint();    // Causing problems...
        }
    }
}
