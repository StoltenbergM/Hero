using UnityEngine;
using UnityEngine.EventSystems;

public class CardDragManager : MonoBehaviour
{
    public static CardDragManager Instance;

    [HideInInspector] public DraggableCardUI draggingCard;

    private void Awake()
    {
        Instance = this;
    }

    public void BeginDrag(DraggableCardUI card)
    {
        draggingCard = card;
    }

    public void EndDrag(DraggableCardUI card, PointerEventData eventData)
    {
        draggingCard = null;

        if (eventData.pointerCurrentRaycast.gameObject == null) 
            return;

        var slot = eventData.pointerCurrentRaycast.gameObject.GetComponent<CardSlotUI>();
        if (slot == null) 
            return;

        AttemptMove(slot);
    }

    private void AttemptMove(CardSlotUI slot)
    {
        TownUI townUI = FindFirstObjectByType<TownUI>();
        var playerDeck = townUI.ActivePlayerDeck;
        var town = townUI.ActiveTown;

        Card card = draggingCard.card;

        // Town → Player
        if (slot.isPlayerSlot)
        {
            CardTransferSystem.MoveCardToPlayer(town, playerDeck, card);
        }
        else // Player → Town
        {
            CardTransferSystem.MoveCardToTown(playerDeck, town, card);
        }

        townUI.RefreshUISlots();
    }
}
