using UnityEngine;

// This script makes a "ScriptableObject" - Each card will be its own .asset file in Unity
// (created via Right Click → Create → Cards → Card Data)

public enum CardTier { I, II, III, IV, V, VI, VII, VIII, XI, X }
public enum CardType { Creature, Magic }
public enum TownFaction { None, Fire, Water, Earth, Air, Shadow, Light }

[CreateAssetMenu(fileName = "NewCard", menuName = "Cards/Card Data")]
public class CardData : ScriptableObject
{
    [Header("Identifiers")]
    public string cardID;   // unique for save/load

    [Header("Basic Info")]
    public string cardName;
    [TextArea] public string description;
    public Sprite artwork;
    
    [Header("Classification")]
    public CardType cardType = CardType.Creature;
    public CardTier tier = CardTier.I;
    public TownFaction faction = TownFaction.None;

    [Header("Stats (for creatures)")]
    public int attack = 0;
    public int defense = 0;
    public int speed = 0;

    [Header("Economy")]
    public int price = 100;

    [Header("Special Ability")]
    public string abilityName;
    [TextArea] public string abilityDescription;
}
