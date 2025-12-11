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

        // Did we drop onto a slot?
        if (eventData.pointerCurrentRaycast.gameObject == null) return;

        var slot = eventData.pointerCurrentRaycast.gameObject.GetComponent<CardSlotUI>();
        if (slot == null) return;

        AttemptMove(slot);
    }

    private void AttemptMove(CardSlotUI slot)
    {
        TownUI townUI = FindObjectOfType<TownUI>();
        PlayerDeck deck = townUI.activePlayerDeck;
        TownController town = townUI.activeTown;

        Card card = draggingCard.card;

        if (slot.isPlayerSlot) // town → player
        {
            CardTransferSystem.MoveCardToPlayer(town, deck, card);
        }
        else // player → town
        {
            CardTransferSystem.MoveCardToTown(deck, town, card);
        }

        townUI.RefreshUISlots();
    }
}
