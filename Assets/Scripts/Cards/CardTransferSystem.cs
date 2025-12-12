using UnityEngine;

public static class CardTransferSystem
{
    // Player → Town
    public static bool MoveCardToTown(PlayerDeck deck, TownController town, Card card)
    {
        if (!deck.ownedCards.Contains(card))
            return false;

        if (!town.HasDefenseSpace())
            return false;

        deck.ownedCards.Remove(card);
        town.AddDefenseCard(card);

        town.ApplyTownBuffs(card);

        return true;
    }

    // Town → Player
    public static bool MoveCardToPlayer(TownController town, PlayerDeck deck, Card card)
    {
        if (!town.defenseDeck.Contains(card))
            return false;

        if (!deck.HasSpace())
            return false;

        town.defenseDeck.Remove(card);
        deck.ownedCards.Add(card);

        card.ResetBonuses();

        return true;
    }
}
