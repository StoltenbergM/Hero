using UnityEngine;
using System.Collections.Generic;

// Defines the static blueprint for each town (faction, levels, cards)
// A SciptableObject of the Town
// Right-click → Create → Towns → Town Data

[CreateAssetMenu(fileName = "NewTown", menuName = "Towns/Town Data")]
public class TownData : ScriptableObject
{
    [Header("Basic Info")]
    public string townName;
    public TownFaction faction;
    public Sprite townImage; // for UI display

    [Header("Upgrade Levels")]
    [Tooltip("Each level unlocks more cards or features")]
    public List<TownLevel> levels = new List<TownLevel>();

    [System.Serializable]
    public class TownLevel
    {
        public string levelName;
        public List<CardData> availableCards = new List<CardData>();
        public int upgradeCost = 1000;
    }
}
