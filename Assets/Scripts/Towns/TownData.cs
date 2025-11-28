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
    public int upgradeCost = 200;
    public int maxLevel = 2;

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

    public List<MagicTier> magicTiers = new List<MagicTier>();

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