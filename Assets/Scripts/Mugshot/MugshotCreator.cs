using UnityEngine;
using UnityEditor;
using UnityEngine.UI; // For UI components
using TMPro; // For TextMeshPro components

/// <summary>
/// A static class responsible for creating UI elements such as character mugshots and solution data slots.
/// </summary>
public static class MugshotCreator
{
    /// <summary>
    /// Creates a character mugshot in the first available inventory slot.
    /// </summary>
    /// <paramname="characterData">The data of the character to create a mugshot for.</param>
    /// <paramname="inventoryManager">The inventory manager responsible for managing inventory slots.</param>
    public static void CreateCharacterMugshot(CharacterData characterData, InventoryManager inventoryManager)
    {
        // Ensure the InventoryManager has a valid Inventory
        if (inventoryManager.Inventory == null)
        {
            Debug.LogError("Inventory GameObject with tag 'Inventory' not found.");
            return;
        }

        // Find the first free InventorySlot (one without children)
        GameObject freeSlot = inventoryManager.FindFirstFreeInventorySlot();

        if (freeSlot == null)
        {
            Debug.LogWarning("No free InventorySlot found.");
            return;
        }

        // Create a new GameObject to serve as the character's mugshot UI element
        GameObject characterMugshot = new GameObject(characterData.name + " Mugshot");
        characterMugshot.transform.SetParent(freeSlot.transform); // Set the parent to the free slot
        characterMugshot.transform.localScale = Vector3.one; // Ensure the scale is uniform

        // Add an InventoryItem component for drag-and-drop functionality
        characterMugshot.AddComponent<InventoryItem>();

        // Add and configure a CanvasGroup component for UI interaction control
        CanvasGroup canvasGroup = characterMugshot.AddComponent<CanvasGroup>();
        canvasGroup.alpha = 1f;
        canvasGroup.interactable = true;
        canvasGroup.blocksRaycasts = true;
        canvasGroup.ignoreParentGroups = false;

        // Add a CharacterInfo component to hold the character data
        CharacterInfo characterInfo = characterMugshot.AddComponent<CharacterInfo>();
        characterInfo.characterData = characterData;

        // Load the texture for the character's image from the specified file path
        string imageFilePath = characterData.image;
        Texture2D texture = AssetDatabase.LoadAssetAtPath<Texture2D>(imageFilePath);

        // Convert the texture to a sprite if it is successfully loaded
        if (texture != null)
        {
            Rect rect = new Rect(0, 0, texture.width, texture.height); // Define the sprite rectangle
            Vector2 pivot = new Vector2(0.5f, 0.5f); // Define the sprite pivot point
            Sprite sprite = Sprite.Create(texture, rect, pivot); // Create the sprite

            // Add an Image component to display the sprite
            Image image = characterMugshot.AddComponent<Image>();
            image.sprite = sprite;
            image.raycastTarget = true;

            // Configure the RectTransform for proper UI positioning
            RectTransform rectTransform = characterMugshot.GetComponent<RectTransform>();
            rectTransform.sizeDelta = new Vector2(150, 150); // Set the size of the mugshot
            rectTransform.localPosition = Vector3.zero; // Center the mugshot in the slot

            // Create a new GameObject for the character's name text
            GameObject textGameObject = new GameObject("NameText");
            textGameObject.transform.SetParent(characterMugshot.transform); // Set parent to the mugshot
            textGameObject.transform.localScale = Vector3.one; // Ensure the scale is uniform

            // Add and configure the TextMeshProUGUI component for displaying the name
            TextMeshProUGUI textMeshPro = textGameObject.AddComponent<TextMeshProUGUI>();
            textMeshPro.text = characterData.name;
            textMeshPro.fontSize = 60;
            textMeshPro.fontStyle = FontStyles.Bold;
            textMeshPro.alignment = TextAlignmentOptions.Center;

            // Configure the RectTransform for the name text positioning
            RectTransform textRectTransform = textGameObject.GetComponent<RectTransform>();
            textRectTransform.sizeDelta = new Vector2(230, 30); // Set the size of the text box
            textRectTransform.localPosition = new Vector3(0, -80, 0); // Position the text below the image
        }
        else
        {
            // Log an error if the texture could not be loaded
            Debug.LogError("Failed to load texture at path: " + imageFilePath);
        }
    }

    /// <summary>
    /// Adds solution data to the first available inventory slot.
    /// </summary>
    /// <param name="solutionData">The data of the solution to add.</param>
    /// <param name="inventoryManager">The inventory manager responsible for managing inventory slots.</param>
    public static void AddSolutionDataToSlot(SolutionData solutionData, InventoryManager inventoryManager)
    {
        // Ensure the InventoryManager has a valid Inventory
        if (inventoryManager.Inventory == null)
        {
            Debug.LogError("Inventory GameObject with tag 'Inventory' not found.");
            return;
        }

        // Find the first free InventorySlot (one without SolutionInfo component)
        GameObject freeSlot = FindFirstFreeSolutionSlot(inventoryManager);

        if (freeSlot == null)
        {
            Debug.LogWarning("No free InventorySlot found.");
            return;
        }

        // Add a SolutionInfo component to the free slot
        SolutionInfo solutionInfo = freeSlot.AddComponent<SolutionInfo>();

        // Assign the solution data to the SolutionInfo component
        solutionInfo.solutionData = solutionData;

        // Log the addition of the solution data to the slot
        Debug.Log("Solution data added to slot: " + freeSlot.name);
    }

    /// <summary>
    /// Finds the first free slot without a SolutionInfo component in the inventory.
    /// </summary>
    /// <param name="inventoryManager">The inventory manager responsible for managing inventory slots.</param>
    /// <returns>The first free inventory slot GameObject, or null if none are available.</returns>
    private static GameObject FindFirstFreeSolutionSlot(InventoryManager inventoryManager)
    {
        // Iterate through all inventory slots
        foreach (GameObject slot in inventoryManager.InventorySlots)
        {
            // Return the slot if it does not have a SolutionInfo component
            if (slot.GetComponent<SolutionInfo>() == null)
            {
                return slot;
            }
        }

        // Return null if no free slot is found
        return null;
    }
}
