using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// A class representing a solution slot that handles the dropping of inventory items into them.
/// </summary>
public class SolutionSlot : MonoBehaviour, IDropHandler
{
    private SolutionInfo solutionInfo; // Reference to the SolutionInfo component attached to this slot
    private CharacterInfo currentCharacter; // Reference to the currently assigned CharacterInfo

    /// <summary>
    /// Called when the script instance is being loaded.
    /// </summary>
    private void Awake()
    {
        solutionInfo = GetComponent<SolutionInfo>(); // Initialize the SolutionInfo component
    }

    /// <summary>
    /// Called when an item is dropped on this slot.
    /// </summary>
    /// <paramname="eventData">Current event data containing information about the drop event.</param>
    public void OnDrop(PointerEventData eventData)
    {
        // Get the InventoryItem component of the dragged item
        InventoryItem draggedItem = eventData.pointerDrag.GetComponent<InventoryItem>();
        if (draggedItem != null)
        {
            // Get the CharacterInfo component of the dragged item
            CharacterInfo characterInfo = draggedItem.GetComponent<CharacterInfo>();

            // Check if the dragged item has CharacterInfo and this slot has SolutionInfo
            if (characterInfo != null && solutionInfo != null)
            {
                // Check if the character data matches the solution data
                if (characterInfo.characterData.name == solutionInfo.solutionData.name &&
                    characterInfo.characterData.sex == solutionInfo.solutionData.sex)
                {
                    // Correct match, snap to the solution slot
                    draggedItem.transform.SetParent(transform);
                    draggedItem.GetComponent<RectTransform>().anchoredPosition = Vector2.zero; // Center the item in the slot
                    currentCharacter = characterInfo; // Update the current character info
                    Debug.Log("Character matched with solution slot.");
                }
                else
                {
                    // Incorrect match, return to the original parent
                    draggedItem.transform.SetParent(draggedItem.OriginalParent);
                    draggedItem.GetComponent<RectTransform>().anchoredPosition = Vector2.zero; // Return the item to its original position
                    Debug.Log("Character did not match with solution slot.");
                }
                // Update the solution checker after every drop
                FindObjectOfType<SolutionChecker>().CheckSolutions();
            }
        }
    }

    /// <summary>
    /// Checks if the solution slot is currently filled with a character.
    /// </summary>
    /// <returns>True if the slot is already filled, false otherwise.</returns>
    public bool IsFilled()
    {
        return currentCharacter != null; // Return true if a character is assigned to this slot
    }
}
