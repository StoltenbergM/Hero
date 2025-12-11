using UnityEngine;

// A lightweight wrapper class that references the CardData.
// This lets players, shops, or battles all use the same CardData, but with their own ownership state.
[System.Serializable]
public class Card
{
    public CardData data;

    // Runtime state (per-player, per-town)
    public int attack;
    public int defense;
    public int speed;
    public int currentHP;

    public bool exhausted = false;

    public Card(CardData data)
    {
        this.data = data;

        // Initialize runtime stats
        attack = data.attack;
        defense = data.defense;
        speed = data.speed;
        currentHP = data.defense; // or HP field if you add one
    }

    public void ResetStats()
    {
        attack = data.attack;
        defense = data.defense;
        speed = data.speed;
        currentHP = data.defense;
    }

    // Helpers
    public int Price => data.price;
    public string Name => data.cardName;
}
