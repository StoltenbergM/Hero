using UnityEngine;
using System.Collections.Generic;
using System.IO;
using System.Linq;

/*
This saves gold, owned cards and active towns for a player
Can expand later with position, magic etc

Place this on a GameObject in your scene called SaveSystem.
Assign:
CardDatabase
PlayerDeck
PlayerEconomy
All your TownController references.

Call SaveSystem.Instance.SaveGame() whenever you want to autosave.
*/

[System.Serializable]
public class SaveData
{
    public int gold;
    public List<string> ownedCardNames = new();
    public List<string> townNames = new();
    public List<int> townLevels = new();
}

public class SaveSystem : MonoBehaviour
{
    public static SaveSystem Instance;
    public CardDatabase cardDatabase;
    public PlayerDeck playerDeck;
    public PlayerEconomy playerEconomy;
    public List<TownController> towns;

    private string savePath => Path.Combine(Application.persistentDataPath, "save.json");

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else Destroy(gameObject);
    }

    public void SaveGame()
    {
        SaveData data = new()
        {
            gold = playerEconomy.gold,
            ownedCardNames = playerDeck.ownedCards.Select(c => c.data.cardName).ToList(),
            townNames = towns.Select(t => t.townData.townName).ToList(),
            townLevels = towns.Select(t => t.currentLevel).ToList()
        };

        File.WriteAllText(savePath, JsonUtility.ToJson(data, true));
        Debug.Log($"Game saved: {savePath}");
    }

    public void LoadGame()
    {
        if (!File.Exists(savePath))
        {
            Debug.LogWarning("No save file found.");
            return;
        }

        var data = JsonUtility.FromJson<SaveData>(File.ReadAllText(savePath));

        playerEconomy.gold = data.gold;
        playerDeck.ownedCards.Clear();

        foreach (string name in data.ownedCardNames)
        {
            var cardData = cardDatabase.GetCardByName(name);
            if (cardData != null)
                playerDeck.AddCard(cardData);
        }

        for (int i = 0; i < data.townNames.Count && i < towns.Count; i++)
        {
            var town = towns.FirstOrDefault(t => t.townData.townName == data.townNames[i]);
            if (town != null)
                town.currentLevel = data.townLevels[i];
        }

        Debug.Log("Game loaded.");
    }
}