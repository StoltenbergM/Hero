using UnityEngine;
using System.Collections.Generic;

// Stores all cards in one place so they can be looked up by name
[CreateAssetMenu(fileName = "CardDatabase", menuName = "Cards/Card Database")]
public class CardDatabase : ScriptableObject
{
    public List<CardData> allCards = new List<CardData>();
    private Dictionary<string, CardData> lookup;

    private void OnEnable()
    {
        lookup = new Dictionary<string, CardData>();
        foreach (var card in allCards)
        {
            if (!lookup.ContainsKey(card.cardName))
                lookup.Add(card.cardName, card);
        }
    }

    public CardData GetCardByName(string name)
    {
        if (lookup.TryGetValue(name, out var card))
            return card;
        Debug.LogWarning($"Card not found: {name}");
        return null;
    }
}
