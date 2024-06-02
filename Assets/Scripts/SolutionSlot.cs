using UnityEngine;
using UnityEngine.EventSystems;

public class SolutionSlot : MonoBehaviour, IDropHandler
{
    private SolutionInfo solutionInfo;
    private CharacterInfo currentCharacter;

    private void Awake()
    {
        solutionInfo = GetComponent<SolutionInfo>();
    }

    public void OnDrop(PointerEventData eventData)
    {
        InventoryItem draggedItem = eventData.pointerDrag.GetComponent<InventoryItem>();
        if (draggedItem != null)
        {
            CharacterInfo characterInfo = draggedItem.GetComponent<CharacterInfo>();

            if (characterInfo != null && solutionInfo != null)
            {
                if (characterInfo.characterData.name == solutionInfo.solutionData.name &&
                    characterInfo.characterData.sex == solutionInfo.solutionData.sex)
                {
                    // Correct match, snap to the solution slot
                    draggedItem.transform.SetParent(transform);
                    draggedItem.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
                    currentCharacter = characterInfo; // Update current character info
                    Debug.Log("Character matched with solution slot.");
                }
                else
                {
                    // Incorrect match, return to the original parent
                    draggedItem.transform.SetParent(draggedItem.OriginalParent);
                    draggedItem.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
                    Debug.Log("Character did not match with solution slot.");
                }
                // Update the solution checker after every drop
                FindObjectOfType<SolutionChecker>().CheckSolutions();
            }
        }
    }

    public bool IsFilled()
    {
        return currentCharacter != null;
    }
}
