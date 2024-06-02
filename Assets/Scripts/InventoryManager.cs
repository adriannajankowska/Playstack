using UnityEngine;

public class InventoryManager
{
    public GameObject Inventory { get; private set; }
    public GameObject[] InventorySlots { get; private set; }

    public void Initialize(string inventoryTag)
    {
        Inventory = GameObject.FindGameObjectWithTag(inventoryTag);
        if (Inventory == null)
        {
            Debug.LogError($"Inventory GameObject with tag '{inventoryTag}' not found.");
            return;
        }

        InventorySlots = new GameObject[Inventory.transform.childCount];
        for (int i = 0; i < Inventory.transform.childCount; i++)
        {
            InventorySlots[i] = Inventory.transform.GetChild(i).gameObject;
        }
    }

    public GameObject FindFirstFreeInventorySlot()
    {
        foreach (GameObject slot in InventorySlots)
        {
            if (slot.transform.childCount == 0)
            {
                return slot;
            }
        }
        return null; // No free slot found
    }
}
