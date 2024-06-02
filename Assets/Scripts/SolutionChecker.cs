using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SolutionChecker : MonoBehaviour
{
    public string solutionInventoryTag = "SolutionInventory"; // Tag for the SolutionInventory GameObject
    public TextMeshProUGUI completionMessage; // Reference to a TextMeshProUGUI component
    public Image completionImage; // Reference to an Image component (optional)

    private SolutionSlot[] solutionSlots;

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

    public void CheckSolutions()
    {
        foreach (SolutionSlot slot in solutionSlots)
        {
            if (!slot.IsFilled())
            {
                // If any slot is not filled, hide the completion indicators and return
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
