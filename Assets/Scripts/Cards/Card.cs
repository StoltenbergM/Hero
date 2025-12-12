using UnityEngine;

// A lightweight wrapper class that references the CardData.
// This lets players, shops, or battles all use the same CardData, but with their own ownership state.
[System.Serializable]
public class Card
{
    public CardData data;

    // Runtime modifiers (not stored on the ScriptableObject)
    public int bonusAttack = 0;
    public int bonusDefense = 0;
    public int bonusSpeed = 0;

    // Optional runtime flags
    public bool isOwned = false;
    public bool isDead = false; // if you later need to mark a card as removed

    public Card(CardData data)
    {
        this.data = data;
    }

    // Final computed stats (base from CardData + runtime bonuses)
    public int Attack => (data != null ? data.attack : 0) + bonusAttack;
    public int Defense => (data != null ? data.defense : 0) + bonusDefense;
    public int Speed => (data != null ? data.speed : 0) + bonusSpeed;

    public int Price => data != null ? data.price : 0;
    public string Name => data != null ? data.cardName : "Unknown";

    // Reset runtime bonuses (used when moving out of a buffing area)
    public void ResetBonuses()
    {
        bonusAttack = 0;
        bonusDefense = 0;
        bonusSpeed = 0;
    }
}
