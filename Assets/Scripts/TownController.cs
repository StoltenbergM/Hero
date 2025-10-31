using UnityEngine;
using System.Collections.Generic;

public class TownController : MonoBehaviour
{
    [Header("Town Settings")]
    public TownData townData;        // Basic info (name, faction, shop list, etc.)
    public int currentLevel = 0;     // Level of the town
    public List<CardData> defenseDeck = new List<CardData>(); // Cards defending the town

    private Node node;               // The map node this town is attached to

    private void Awake()
    {
        // Try to find the Node component on the same GameObject
        node = GetComponent<Node>();
        if (node != null)
        {
            node.nodeType = Node.NodeType.Town;
        }
    }

    public List<CardData> GetAvailableCards()
    {
        if (townData == null) return new List<CardData>();
        return townData.GetCardsForLevel(currentLevel);
    }

    public void LevelUp()
    {
        if (townData == null) return;
        currentLevel = Mathf.Min(currentLevel + 1, townData.maxLevel - 1);
    }
}
