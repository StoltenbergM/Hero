using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

/* 
Notes & improvements you can do later:

Replace the quick TMP_Text creation in PopulateDefense with a proper prefab for defense slots.
Add confirmation for buying (use UIManager.ShowConfirm()).
Add a sell/remove button for defense cards.
*/

public class TownUI : MonoBehaviour
{
    [Header("UI References")]
    public GameObject panel; // root of town UI (set inactive by default)
    public TMP_Text townNameText;
    public TMP_Text factionText;
    public TMP_Text levelText;
    public TMP_Text playerGoldText;

    public Transform shopListParent; // content transform where ShopItemUI prefabs go
    public ShopItemUI shopItemUIPrefab;

    public Transform townDefenseCardsParent; // where defense slots are shown (simple TMP or images)

    private TownController activeTown;
    private PlayerDeck activePlayerDeck;
    private PlayerEconomy activeEconomy;

    public void ShowTown(TownController town, PlayerDeck playerDeck, PlayerEconomy economy)
    {
        activeTown = town;
        activePlayerDeck = playerDeck;
        activeEconomy = economy;

        panel.SetActive(true);
        RefreshUI();
    }

    public void CloseTown()
    {
        panel.SetActive(false);
        ClearShopList();
    }

    private void RefreshUI()
    {
        if (activeTown == null) return;

        if (townNameText != null) townNameText.text = activeTown.townData.townName;
        if (factionText != null) factionText.text = activeTown.townData.faction.ToString();
        if (levelText != null) levelText.text = $"Level {activeTown.currentLevel + 1}";
        if (playerGoldText != null)
            playerGoldText.text = (activeEconomy != null) ? $"Gold: {activeEconomy.gold}" : "Gold: ?";

        PopulateShop();
        PopulateDefense();
    }

    private void PopulateShop()
    {
        ClearShopList();

        // Cards
        List<CardData> available = activeTown.GetAvailableCards();
        foreach (var cardData in available)
        {
            var item = Instantiate(shopItemUIPrefab, shopListParent);
            item.Setup(cardData, this);
        }

        // Town Upgrade option
        if (activeTown.CanUpgrade())
        {
            var upgradeButton = Instantiate(shopItemUIPrefab, shopListParent);
            var fakeCard = ScriptableObject.CreateInstance<CardData>();
            fakeCard.cardName = "Upgrade Town";
            fakeCard.price = activeTown.townData.upgradeCost;
            upgradeButton.Setup(fakeCard, this);
        }
    }

    private void ClearShopList()
    {
        for (int i = shopListParent.childCount - 1; i >= 0; i--)
            Destroy(shopListParent.GetChild(i).gameObject);
    }

    private void PopulateDefense()
    {
        // simple text list for defense cards (you can replace with slots + drag/drop later)
        for (int i = townDefenseCardsParent.childCount - 1; i >= 0; i--)
            Destroy(townDefenseCardsParent.GetChild(i).gameObject);

        if (activePlayerDeck == null) return;
        
        foreach (var card in activeTown.defenseDeck)
        {
            var go = new GameObject("DefenseEntry");
            var text = go.AddComponent<TMP_Text>(); // quick and dirty; prefer a prefab for real UI
            text.text = card.cardName;
            go.transform.SetParent(townDefenseCardsParent, false);
        }
    }

    // Card buying logic
    private void OnBuyCard(CardData data)
    {
        if (activeEconomy == null || activePlayerDeck == null)
        {
            Debug.LogWarning("Missing player deck or economy reference.");
            return;
        }

        if (activeEconomy.gold < data.price)
        {
            Debug.Log("Not enough gold!");
            return;
        }

        // Spend gold
        if (activeEconomy.Spend(data.price))
        {
            activePlayerDeck.AddCard(data);
            Debug.Log($"Bought {data.cardName} for {data.price} gold.");

            // Update UI
            RefreshUI();
        }
    }

    public void TryBuyCard(CardData data)
    {
        if (activeEconomy.gold < data.price)
        {
            Debug.Log("Not enough gold!");
            return;
        }

        if (activeEconomy.Spend(data.price))
        {
            activePlayerDeck.AddCard(data);
            Debug.Log($"Bought {data.cardName} for {data.price} gold!");
            RefreshUI();
        }

        if (data.cardName == "Upgrade Town")
        {
        if (activeEconomy.Spend(activeTown.townData.upgradeCost))
            {
                activeTown.UpgradeTown();
                RefreshUI();
            }
            return;
        }
    }
}
