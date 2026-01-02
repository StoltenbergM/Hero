using UnityEngine;
using System.Collections.Generic;

// Defines the static blueprint for each town (faction, levels, cards)
// A SciptableObject of the Town
// Right-click → Create → Towns → Town Data


[CreateAssetMenu(fileName = "NewTownData", menuName = "Game/Town Data")]
public class TownData : ScriptableObject
{
    [Header("Town Info")]
    public string townName;
    public Faction faction; // e.g. Fire, Water, Earth, etc.
    public List<MagicTier> magicTiers;   // unlockable via magic level
    public int upgradeCost = 200;
    
    [Header("Faction Creatures (always 7)")]
    public List<CardData> creatureCards;

    [Header("Defense slots per level")]
    public int[] defenseSlotsPerLevel = { 5, 7 };

    [Header("Town progression")]
    public int maxLevel => defenseSlotsPerLevel.Length; // lenght derived from lenght of the list above

    [Header("Shop Inventory by Level")]
    public List<LevelShop> levels = new List<LevelShop>();

    [System.Serializable]
    public class LevelShop
    {
        public List<CardData> availableCards;
    }

    [System.Serializable]
    public class MagicTier
    {
        public int requiredMagicLevel;
        public List<CardData> magicCards;
    }

    // Get the cards available for a given town level
    public List<CardData> GetCardsForLevel(int level)
    {
        if (levels == null || levels.Count == 0)
            return new List<CardData>();

        int safeLevel = Mathf.Clamp(level, 0, levels.Count - 1);
        return levels[safeLevel].availableCards;
    }
}

public enum Faction
{
    Neutral,
    Fire,
    Water,
    Earth,
    Air,
    Shadow
}