using UnityEngine;
using UnityEngine.UI;

public class CardSlotUI : MonoBehaviour
{
    public Image slotHighlight;
    public bool isPlayerSlot;
    public int slotIndex;

    private Card storedCard;

    public bool HasCard => storedCard != null;

    public void SetCard(Card card)
    {
        storedCard = card;
    }

    public Card GetCard()
    {
        return storedCard;
    }

    public void Clear()
    {
        storedCard = null;
    }

    public void Highlight(bool on)
    {
        if (slotHighlight != null)
            slotHighlight.enabled = on;
    }
}
