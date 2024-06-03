using UnityEngine;
using UnityEditor;

/// <summary>
/// Custom editor for CharacterData ScriptableObject, allowing creating a visual Character mugshot via Inspector.
/// </summary>
[CustomEditor(typeof(CharacterData))]
public class CharacterMugshotEditor : Editor
{
    // Inventory manager to manage character inventory items
    private InventoryManager inventoryManager;
    // Default tag for the inventory
    private string inventoryTag = "CharacterInventory";

    /// <summary>
    /// Called when the editor is enabled. Initializes the inventory manager.
    /// </summary>
    private void OnEnable()
    {
        // Create a new instance of InventoryManager
        inventoryManager = new InventoryManager();
        // Initialize the inventory manager with the default tag
        inventoryManager.Initialize(inventoryTag);
    }

    /// <summary>
    /// Overrides the default inspector GUI to add custom functionality.
    /// </summary>
    public override void OnInspectorGUI()
    {
        // Get the target CharacterData being inspected
        CharacterData characterData = (CharacterData)target;

        // Draw the default inspector fields for CharacterData
        DrawDefaultInspector();

        // Add some space before the custom button
        GUILayout.Space(10);
        // Label for the custom functionality
        GUILayout.Label("Create a Mugshot of a Character from its ScriptableObject", EditorStyles.boldLabel);

        // Button to create a character mugshot
        if (GUILayout.Button("Create Character Mugshot"))
        {
            // Call the MugshotCreator to create a mugshot for the character
            MugshotCreator.CreateCharacterMugshot(characterData, inventoryManager);
        }

        // Repaint the editor to avoid layout issues
        if (Event.current.type == EventType.Repaint)
        {
            // Reset the label width
            EditorGUIUtility.labelWidth = 0;
            // Repaint the focused window (commented out due to potential issues)
            //EditorWindow.focusedWindow.Repaint();
        }
    }
}
