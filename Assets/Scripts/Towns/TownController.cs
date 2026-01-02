using UnityEngine;
using System.Collections.Generic;

public class TownController : MonoBehaviour
{
    [Header("Town Settings")]
    public PlayerProgress owner;
    public TownData townData;
    public int currentLevel = 0;

    // Runtime cards defending this town
    public List<Card> defenseDeck = new List<Card>();

    [Header("Limits")]
    public int maxDefenseSlots => townData.defenseSlotsPerLevel[currentLevel];

    [Header("Costs")]
    public int upgradePrice = 200;
    public int boatPrice = 150;

    [Header("Town Buffs")]
    public int bonusAttack = 0;
    public int bonusDefense = 2;   // Example buff
    public int bonusSpeed = 0;

    private Node node;

    private void Awake()
    {
        node = GetComponent<Node>();
        if (node != null)
            node.nodeType = Node.NodeType.Town;
    }

    public bool CanUpgrade() => currentLevel < townData.maxLevel;

    public void UpgradeTown()
    {
        if (CanUpgrade())
        {
            currentLevel++;
            Debug.Log($"{townData.townName} upgraded to level {currentLevel + 1}");
        }
    }

    // ------------------------------
    // CARD MANAGEMENT
    // ------------------------------

    public bool HasDefenseSpace() => defenseDeck.Count < maxDefenseSlots;

    public bool CanBuyCard(CardData card)
    {
        // Level 1 → first 5 cards
        if (currentLevel == 0)
            return townData.creatureCards.IndexOf(card) < 5;

        // Level 2 → all cards
        return true;
    }

    public bool AddDefenseCard(Card card)
    {
        if (!HasDefenseSpace())
        {
            Debug.Log("Town defense is full!");
            return false;
        }

        defenseDeck.Add(card);
        return true;
    }

    public bool RemoveDefenseCard(Card card)
    {
        return defenseDeck.Remove(card);
    }

    // ------------------------------
    // STAT BUFF APPLY
    // ------------------------------

    public void ApplyTownBuffs(Card card)
    {
        card.bonusAttack += bonusAttack;
        card.bonusDefense += bonusDefense;
        card.bonusSpeed += bonusSpeed;
    }
}
