using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventoryItem : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    private RectTransform rectTransform;
    private Canvas canvas;
    private CanvasGroup canvasGroup;
    public Transform OriginalParent { get; private set; }

    private GameObject tooltip; // Reference to the tooltip GameObject
    private Text tooltipText; // Reference to the Text component of the tooltip

    private CharacterInfo characterInfo; // Reference to the CharacterInfo component on the game object
    private float tooltipOffset = 270f; // Offset to position the tooltip to the right of the draggable object

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

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (canvasGroup != null)
        {
            canvasGroup.alpha = 0.6f; // Make the UI element semi-transparent during drag
            canvasGroup.blocksRaycasts = false; // Prevents the UI element from blocking raycasts while dragging
        }
        OriginalParent = transform.parent;
        transform.SetParent(transform.root);
        transform.SetAsLastSibling();

        if (tooltip != null)
        {
            UpdateTooltipText();
            // Show the tooltip text
            tooltipText.enabled = true;
            PositionTooltip();
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (rectTransform != null && canvas != null)
        {
            Vector2 localPointerPosition;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                canvas.transform as RectTransform,
                eventData.position,
                eventData.pressEventCamera,
                out localPointerPosition);
            rectTransform.anchoredPosition += eventData.delta / canvas.scaleFactor;
        }
        if (tooltip != null)
        {
            PositionTooltip();
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (canvasGroup != null)
        {
            canvasGroup.alpha = 1.0f; // Reset the UI element to fully opaque
            canvasGroup.blocksRaycasts = true; // Allow the UI element to block raycasts again
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

    private void PositionTooltip()
    {
        // Copy the position of the draggable object
        tooltip.transform.position = transform.position;
    }
}
