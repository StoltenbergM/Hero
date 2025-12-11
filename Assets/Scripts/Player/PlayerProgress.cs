using UnityEngine;
using System.Collections.Generic;

public class PlayerProgress : MonoBehaviour
{
    [Header("General Resources")]
    public int gold = 0;

    [Header("Abilities / Unlocks")]
    public int magicLevel = 1;
    public int maxMagicLevel = 3;
    public bool hasBoat = false;

    [Header("Owned Assets (runtime instances)")]
    public List<Card> ownedCards = new List<Card>();   // runtime card instances
    public List<TownController> ownedTowns = new List<TownController>();

    [Header("References (assign or auto-find)")]
    public PlayerDeck deck;
    // PlayerEconomy can be removed if you centralize gold here, otherwise keep for compatibility
    public PlayerEconomy economy;

    private void Awake()
    {
        if (deck == null) deck = GetComponent<PlayerDeck>();
        if (economy == null) economy = GetComponent<PlayerEconomy>();
    }

    // ----------------------------
    // GOLD / ECONOMY
    // ----------------------------
    public bool SpendGold(int amount)
    {
        if (gold < amount) return false;
        gold -= amount;
        return true;
    }

    public void AddGold(int amount)
    {
        gold += amount;
    }

    // ----------------------------
    // MAGIC PROGRESSION
    // ----------------------------
    public bool CanUpgradeMagic() => magicLevel < maxMagicLevel;

    public bool TryUpgradeMagic()
    {
        if (!CanUpgradeMagic()) return false;
        magicLevel++;
        return true;
    }

    // ----------------------------
    // CARD OWNERSHIP (runtime instance)
    // ----------------------------
    public Card AddCard(CardData cardData)
    {
        if (cardData == null) return null;

        Card newCard = new Card(cardData);
        ownedCards.Add(newCard);

        // Keep the PlayerDeck (UI/usage) in sync if present
        if (deck != null)
        {
            if (deck.ownedCards == null) deck.ownedCards = new System.Collections.Generic.List<Card>();
            deck.ownedCards.Add(newCard);
        }

        return newCard;
    }

    public bool RemoveCard(Card card)
    {
        if (card == null) return false;

        bool removed = ownedCards.Remove(card);

        if (deck != null && deck.ownedCards != null)
            deck.ownedCards.Remove(card);

        return removed;
    }

    // Optionally remove by CardData (remove the first matching instance)
    public bool RemoveCardByData(CardData data)
    {
        var found = ownedCards.Find(c => c.data == data);
        if (found == null) return false;
        return RemoveCard(found);
    }

    // ----------------------------
    // TOWN OWNERSHIP
    // ----------------------------
    public void CaptureTown(TownController town)
    {
        if (!ownedTowns.Contains(town))
            ownedTowns.Add(town);

        town.owner = this;
    }

    public bool OwnsTown(TownController town) => ownedTowns.Contains(town);
}
