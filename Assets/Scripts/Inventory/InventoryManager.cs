using UnityEngine;

/// <summary>
/// Manages the inventory by handling the inventory GameObject and its slots.
/// </summary>
public class InventoryManager
{
    // Reference to the inventory GameObject
    public GameObject Inventory { get; private set; }
    // Array of GameObjects representing the inventory slots
    public GameObject[] InventorySlots { get; private set; }

    /// <summary>
    /// Initializes the InventoryManager by finding the inventory GameObject on the scene using the specified tag.
    /// </summary>
    /// <paramname="inventoryTag">The tag used to find the inventory GameObject.</param>
    public void Initialize(string inventoryTag)
    {
        // Find the inventory GameObject by its tag
        Inventory = GameObject.FindGameObjectWithTag(inventoryTag);
        if (Inventory == null)
        {
            // Log an error if the inventory GameObject is not found
            Debug.LogError($"Inventory GameObject with tag '{inventoryTag}' not found.");
            return;
        }

        // Initialize the array to hold references to the inventory slots
        InventorySlots = new GameObject[Inventory.transform.childCount];
        // Populate the array with child GameObjects of the inventory
        for (int i = 0; i < Inventory.transform.childCount; i++)
        {
            InventorySlots[i] = Inventory.transform.GetChild(i).gameObject;
        }
    }

    /// <summary>
    /// Finds the first free inventory slot (i.e., a slot with no child objects).
    /// </summary>
    /// <returns>The first free inventory slot GameObject, or null if no free slot is found.</returns>
    public GameObject FindFirstFreeInventorySlot()
    {
        // Iterate through each slot in the inventory
        foreach (GameObject slot in InventorySlots)
        {
            // Check if the slot has no child objects, indicating it's free
            if (slot.transform.childCount == 0)
            {
                return slot; // Return the first free slot found
            }
        }
        return null; // Return null if no free slot is found
    }
}
