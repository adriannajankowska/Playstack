using UnityEngine;
using UnityEditor;
using UnityEngine.UI; // For UI components
using TMPro; // For TextMeshPro components

public static class MugshotCreator
{
    public static void CreateCharacterMugshot(CharacterData characterData, InventoryManager inventoryManager)
    {
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

        // Create a new GameObject as a UI element
        GameObject characterMugshot = new GameObject(characterData.name + " Mugshot");
        characterMugshot.transform.SetParent(freeSlot.transform);
        characterMugshot.transform.localScale = Vector3.one;

        // Add DragDrop component
        characterMugshot.AddComponent<InventoryItem>();

        // Add CanvasGroup component
        CanvasGroup canvasGroup = characterMugshot.AddComponent<CanvasGroup>();
        canvasGroup.alpha = 1f;
        canvasGroup.interactable = true;
        canvasGroup.blocksRaycasts = true;
        canvasGroup.ignoreParentGroups = false;

        // Create a CharacterInfo object containing the original character data (pointer to a ScriptableObject file)
        CharacterInfo characterInfo = characterMugshot.AddComponent<CharacterInfo>();
        characterInfo.characterData = characterData;

        // Load the texture from the file path
        string imageFilePath = characterData.image;
        Texture2D texture = AssetDatabase.LoadAssetAtPath<Texture2D>(imageFilePath);

        // Convert the texture to a sprite
        if (texture != null)
        {
            Rect rect = new Rect(0, 0, texture.width, texture.height);
            Vector2 pivot = new Vector2(0.5f, 0.5f);
            Sprite sprite = Sprite.Create(texture, rect, pivot);

            // Add an Image component and assign the sprite
            Image image = characterMugshot.AddComponent<Image>();
            image.sprite = sprite;
            image.raycastTarget = true;

            // Set up RectTransform for UI positioning
            RectTransform rectTransform = characterMugshot.GetComponent<RectTransform>();
            rectTransform.sizeDelta = new Vector2(150, 150);
            rectTransform.localPosition = Vector3.zero;

            // Create and position the TextMeshProUGUI component for the character's name
            GameObject textGameObject = new GameObject("NameText");
            textGameObject.transform.SetParent(characterMugshot.transform);
            textGameObject.transform.localScale = Vector3.one;
            TextMeshProUGUI textMeshPro = textGameObject.AddComponent<TextMeshProUGUI>();
            textMeshPro.text = characterData.name;
            textMeshPro.fontSize = 60;
            textMeshPro.fontStyle = FontStyles.Bold;
            textMeshPro.alignment = TextAlignmentOptions.Center;

            // Set up RectTransform for the text
            RectTransform textRectTransform = textGameObject.GetComponent<RectTransform>();
            textRectTransform.sizeDelta = new Vector2(230, 30);
            textRectTransform.localPosition = new Vector3(0, -80, 0); // Position the text below the image
        }
        else
        {
            Debug.LogError("Failed to load texture at path: " + imageFilePath);
        }
    }

    public static void AddSolutionDataToSlot(SolutionData solutionData, InventoryManager inventoryManager)
    {
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

        // Add the SolutionInfo component directly to the slot
        SolutionInfo solutionInfo = freeSlot.AddComponent<SolutionInfo>();

        // Update the slot with the solution data
        solutionInfo.solutionData = solutionData;

        Debug.Log("Solution data added to slot: " + freeSlot.name);
    }

    private static GameObject FindFirstFreeSolutionSlot(InventoryManager inventoryManager)
    {
        foreach (GameObject slot in inventoryManager.InventorySlots)
        {
            if (slot.GetComponent<SolutionInfo>() == null)
            {
                return slot;
            }
        }
        return null; // No free slot found
    }
}
