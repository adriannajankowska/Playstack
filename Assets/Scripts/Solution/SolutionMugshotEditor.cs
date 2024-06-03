using UnityEngine;
using UnityEditor;

/// <summary>
/// Custom editor for SolutionData ScriptableObject, allowing adding SolutionInfo component to the Solution Slot.
/// </summary>
[CustomEditor(typeof(SolutionData))]
public class SolutionMugshotEditor : Editor
{
    // Inventory manager to manage solution inventory items
    private InventoryManager inventoryManager;
    // Default tag for the inventory
    private string inventoryTag = "SolutionInventory";

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
        // Get the target SolutionData being inspected
        SolutionData solutionData = (SolutionData)target;

        // Draw the default inspector fields for SolutionData
        DrawDefaultInspector();

        // Add some space before the custom button
        GUILayout.Space(10);
        // Label for the custom functionality
        GUILayout.Label("Add Solution Data to Inventory Slot", EditorStyles.boldLabel);

        // Button to add solution data to inventory slot
        if (GUILayout.Button("Add Solution Data to Slot"))
        {
            // Call the MugshotCreator to add solution data to an inventory slot
            MugshotCreator.AddSolutionDataToSlot(solutionData, inventoryManager);
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
