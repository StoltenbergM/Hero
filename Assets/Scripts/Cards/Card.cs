using UnityEngine;

// A lightweight wrapper class that references the CardData.
// This lets players, shops, or battles all use the same CardData, but with their own ownership state.

[System.Serializable]
public class Card
{
    public CardData data;
    public bool isOwned = false;

    public Card(CardData data)
    {
        this.data = data;
    }

    public int Attack => data.attack;
    public int Defense => data.defense;
    public int Speed => data.speed;
    public int Price => data.price;
    public string Name => data.cardName;
}
