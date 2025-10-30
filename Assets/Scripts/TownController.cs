using UnityEngine;
using System.Collections.Generic;

// Runtime manager for ownership, upgrades, defense deck
// Script for each Town instance

public class TownController : MonoBehaviour
{
    [Header("Town Setup")]
    public TownData townData;
    public int currentLevel = 0;

    [Header("Ownership")]
    public PlayerDeck owner; // player who owns the town
    public bool isOccupied => owner != null;

    [Header("Defense Deck")]
    public List<Card> defenseDeck = new List<Card>();
    public int maxDefenseSlots = 5;

    public TownData.TownLevel CurrentLevelData =>
        (townData != null && currentLevel < townData.levels.Count)
        ? townData.levels[currentLevel]
        : null;

    public void AssignOwner(PlayerDeck newOwner)
    {
        owner = newOwner;
        Debug.Log($"{townData.townName} is now owned by {owner.gameObject.name}");
    }

    public void UpgradeTown()
    {
        if (townData == null || currentLevel >= townData.levels.Count - 1)
        {
            Debug.LogWarning("Town already maxed out or not configured.");
            return;
        }

        currentLevel++;
        Debug.Log($"{townData.townName} upgraded to level {currentLevel + 1}!");
    }

    public List<CardData> GetAvailableCards()
    {
        return CurrentLevelData?.availableCards ?? new List<CardData>();
    }

    public void AddDefenseCard(Card card)
    {
        if (defenseDeck.Count >= maxDefenseSlots)
        {
            Debug.LogWarning("Defense deck full!");
            return;
        }
        defenseDeck.Add(card);
        Debug.Log($"Added {card.data.cardName} to {townData.townName}'s defense.");
    }

    public void RemoveDefenseCard(Card card)
    {
        defenseDeck.Remove(card);
    }
}
