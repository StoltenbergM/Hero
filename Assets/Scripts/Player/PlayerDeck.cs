using UnityEngine;
using System.Collections.Generic;

// Each players deck/inventory of cards

public class PlayerDeck : MonoBehaviour
{
    public List<Card> ownedCards = new List<Card>();
    public int maxDeckSize = 5;
    public bool HasSpace() => ownedCards.Count < maxDeckSize;

    public void AddCard(CardData cardData)
    {
        ownedCards.Add(new Card(cardData));
        Debug.Log($"Added {cardData.cardName} to deck.");
    }

    public void RemoveCard(Card card)
    {
        ownedCards.Remove(card);
        Debug.Log($"Removed {card.data.cardName} from deck.");
    }

    public Card GetStrongestCard()
    {
        if (ownedCards.Count == 0) return null;
        Card best = ownedCards[0];
        foreach (Card c in ownedCards)
        {
            if (c.Attack + c.Defense + c.Speed > best.Attack + best.Defense + best.Speed)
                best = c;
        }
        return best;
    }
}
