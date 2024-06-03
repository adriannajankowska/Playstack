using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(SolutionData))]
public class SolutionMugshotEditor : Editor
{
    private InventoryManager inventoryManager;
    private string inventoryTag = "SolutionInventory"; // Set the default tag for solution inventory

    private void OnEnable()
    {
        inventoryManager = new InventoryManager();
        inventoryManager.Initialize(inventoryTag);
    }

    public override void OnInspectorGUI()
    {
        // Reference to the target object being inspected
        SolutionData solutionData = (SolutionData)target;

        // Display the default inspector fields for the SolutionData component
        DrawDefaultInspector();

        GUILayout.Space(10);
        GUILayout.Label("Add Solution Data to Inventory Slot", EditorStyles.boldLabel);

        if (GUILayout.Button("Add Solution Data to Slot"))
        {
            MugshotCreator.AddSolutionDataToSlot(solutionData, inventoryManager);
        }

        // Repaint the editor to avoid layout issues
        if (Event.current.type == EventType.Repaint)
        {
            EditorGUIUtility.labelWidth = 0; // Reset label width
            EditorWindow.focusedWindow.Repaint();
        }
    }
}
