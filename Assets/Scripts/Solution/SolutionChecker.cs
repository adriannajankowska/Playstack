using UnityEngine;
using UnityEngine.UI;
using TMPro;

/// <summary>
/// Class responsible for checking if all solution slots are correctly filled and displaying a completion message.
/// </summary>
public class SolutionChecker : MonoBehaviour
{
    public string solutionInventoryTag = "SolutionInventory"; // Tag for the SolutionInventory GameObject
    public TextMeshProUGUI completionMessage; // Reference to a TextMeshProUGUI component for displaying completion message
    public Image completionImage; // Reference to an Image component for displaying a completion image (optional)

    private SolutionSlot[] solutionSlots; // Array of SolutionSlot components in the solution inventory

    /// <summary>
    /// Called when the script instance is being loaded.
    /// </summary>
    private void Start()
    {
        // Find the SolutionInventory GameObject by tag
        GameObject solutionInventory = GameObject.FindGameObjectWithTag(solutionInventoryTag);
        if (solutionInventory == null)
        {
            Debug.LogError($"SolutionInventory GameObject with tag '{solutionInventoryTag}' not found.");
            return;
        }

        // Get all SolutionSlot components from the children of the SolutionInventory GameObject
        solutionSlots = solutionInventory.GetComponentsInChildren<SolutionSlot>();

        if (solutionSlots.Length == 0)
        {
            Debug.LogWarning("No SolutionSlot components found in SolutionInventory children.");
        }

        // Initially hide the completion message and image
        if (completionMessage != null)
        {
            completionMessage.gameObject.SetActive(false);
        }

        if (completionImage != null)
        {
            completionImage.gameObject.SetActive(false);
        }
    }

    /// <summary>
    /// Checks if all solution slots are filled and displays the completion indicators if they are.
    /// </summary>
    public void CheckSolutions()
    {
        // Iterate through each solution slot
        foreach (SolutionSlot slot in solutionSlots)
        {
            // If any slot is not filled, hide the completion indicators and return
            if (!slot.IsFilled())
            {
                if (completionMessage != null)
                {
                    completionMessage.gameObject.SetActive(false);
                }

                if (completionImage != null)
                {
                    completionImage.gameObject.SetActive(false);
                }

                return;
            }
        }

        // If all slots are filled, show the completion indicators
        if (completionMessage != null)
        {
            completionMessage.gameObject.SetActive(true);
        }

        if (completionImage != null)
        {
            completionImage.gameObject.SetActive(true);
        }
    }
}
