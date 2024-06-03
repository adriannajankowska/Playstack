using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

/// <summary>
/// Handles the dragging functionality for inventory items, including displaying a tooltip.
/// </summary>
public class InventoryItem : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    // Reference to the RectTransform component of this UI element
    private RectTransform rectTransform;
    // Reference to the Canvas component in the parent hierarchy
    private Canvas canvas;
    // Reference to the CanvasGroup component to control the UI element's transparency and raycast blocking
    private CanvasGroup canvasGroup;
    // Original parent of the draggable item to return to if dropped incorrectly
    public Transform OriginalParent { get; private set; }

    // Tooltip GameObject and its Text component
    private GameObject tooltip;
    private Text tooltipText;

    // Reference to the CharacterInfo component on the game object
    private CharacterInfo characterInfo;
    // Offset to position the tooltip to the right of the draggable object
    private float tooltipOffset = 270f;

    /// <summary>
    /// Initializes references to required components.
    /// </summary>
    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        canvas = GetComponentInParent<Canvas>();
        canvasGroup = GetComponent<CanvasGroup>();

        // Find the tooltip GameObject by its tag
        tooltip = GameObject.FindWithTag("CharacterInfoTooltip");
        if (tooltip != null)
        {
            tooltipText = tooltip.GetComponentInChildren<Text>();
            // Initially hide the tooltip text
            tooltipText.enabled = false;
        }
        else
        {
            Debug.LogError("Tooltip GameObject with tag 'CharacterInfoTooltip' not found.");
        }

        // Get the CharacterInfo component
        characterInfo = GetComponent<CharacterInfo>();
        if (characterInfo == null)
        {
            Debug.LogError("CharacterInfo component not found on this GameObject.");
        }
    }

    /// <summary>
    /// Called when the user begins dragging the UI element.
    /// </summary>
    /// <paramname="eventData">Event data for the drag.</param>
    public void OnBeginDrag(PointerEventData eventData)
    {
        if (canvasGroup != null)
        {
            // Make the UI element semi-transparent during drag
            canvasGroup.alpha = 0.6f;
            // Prevents the UI element from blocking raycasts while dragging
            canvasGroup.blocksRaycasts = false;
        }
        // Store the original parent to return to if dropped incorrectly
        OriginalParent = transform.parent;
        // Temporarily set the parent to the root to ensure it appears on top of other UI elements
        transform.SetParent(transform.root);
        // Ensure the element is rendered last (on top)
        transform.SetAsLastSibling();

        if (tooltip != null)
        {
            UpdateTooltipText();
            // Show the tooltip text
            tooltipText.enabled = true;
            // Position the tooltip correctly
            PositionTooltip();
        }
    }

    /// <summary>
    /// Called repeatedly while the user is dragging the UI element.
    /// </summary>
    /// <paramname="eventData">Event data for the drag.</param>
    public void OnDrag(PointerEventData eventData)
    {
        if (rectTransform != null && canvas != null)
        {
            // Convert screen point to local point in the canvas
            Vector2 localPointerPosition;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                canvas.transform as RectTransform,
                eventData.position,
                eventData.pressEventCamera,
                out localPointerPosition);
            // Update the position of the UI element based on the drag delta
            rectTransform.anchoredPosition += eventData.delta / canvas.scaleFactor;
        }
        if (tooltip != null)
        {
            // Position the tooltip correctly
            PositionTooltip();
        }
    }

    /// <summary>
    /// Called when the user ends dragging the UI element.
    /// </summary>
    /// <paramname="eventData">Event data for the drag.</param>
    public void OnEndDrag(PointerEventData eventData)
    {
        if (canvasGroup != null)
        {
            // Reset the UI element to fully opaque
            canvasGroup.alpha = 1.0f;
            // Allow the UI element to block raycasts again
            canvasGroup.blocksRaycasts = true;
        }

        // Snap back to original position if not dropped on a valid slot
        if (transform.parent == transform.root)
        {
            transform.SetParent(OriginalParent);
            rectTransform.anchoredPosition = Vector2.zero;
        }

        if (tooltip != null)
        {
            // Hide the tooltip text
            tooltipText.enabled = false;
        }
    }

    /// <summary>
    /// Updates the tooltip text with the currently dragged character's items and their prices.
    /// </summary>
    private void UpdateTooltipText()
    {
        if (characterInfo != null && characterInfo.characterData != null)
        {
            CharacterData characterData = characterInfo.characterData;
            string itemsText = "Items Owned:\n";
            foreach (var item in characterData.itemsOwned)
            {
                itemsText += $"{item.itemName}: {item.price} gold\n";
            }
            tooltipText.text = itemsText;
        }
        else
        {
            tooltipText.text = "No items found.";
        }
    }

    /// <summary>
    /// Positions the tooltip next to the draggable object.
    /// </summary>
    private void PositionTooltip()
    {
        // Set the tooltip's position to be next to the draggable object
        tooltip.transform.position = transform.position;
    }
}
