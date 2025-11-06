using UnityEngine;
using System.IO;

public static class SaveSystem
{
    private static string savePath => Application.persistentDataPath + "/save.json";

    [System.Serializable]
    private class SaveData
    {
        public int gold;
        public string[] ownedCards;
    }

    public static void Save(PlayerEconomy economy, PlayerDeck deck)
    {
        SaveData data = new SaveData
        {
            gold = economy.gold,
            ownedCards = deck.ownedCards.ConvertAll(c => c.data.cardName).ToArray()
        };

        File.WriteAllText(savePath, JsonUtility.ToJson(data, true));
        Debug.Log("💾 Game saved to " + savePath);
    }

    public static void Load(PlayerEconomy economy, PlayerDeck deck)
    {
        if (!File.Exists(savePath))
        {
            Debug.Log("No save file found.");
            return;
        }

        SaveData data = JsonUtility.FromJson<SaveData>(File.ReadAllText(savePath));
        economy.gold = data.gold;

        deck.ownedCards.Clear();
        foreach (var cardName in data.ownedCards)
        {
            // You’ll later replace this lookup with a CardDatabase reference
            CardData cardData = Resources.Load<CardData>($"Cards/{cardName}");
            if (cardData != null)
                deck.AddCard(cardData);
        }

        Debug.Log("✅ Game loaded.");
    }
}
