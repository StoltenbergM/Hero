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

    public Transform shopListParent; // content transform where ShopCardItem prefabs go
    public ShopCardUI shopCardPrefab;

    public Transform defenseListParent; // where defense slots are shown (simple TMP or images)

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
        if (playerGoldText != null) playerGoldText.text = $"Gold: {activeEconomy.gold}";

        PopulateShop();
        PopulateDefense();
    }

    private void PopulateShop()
    {
        ClearShopList();

        List<CardData> avail = activeTown.GetAvailableCards();
        foreach (var cardData in avail)
        {
            var item = Instantiate(shopCardPrefab, shopListParent);
            bool canBuy = activeEconomy.gold >= cardData.price && (activePlayerDeck.ownedCards.Count < 10); // example cap
            item.Setup(cardData, OnBuyCard, canBuy);
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
        for (int i = defenseListParent.childCount - 1; i >= 0; i--)
            Destroy(defenseListParent.GetChild(i).gameObject);

        foreach (var card in activeTown.defenseDeck)
        {
            var go = new GameObject("DefenseEntry");
            var text = go.AddComponent<TMP_Text>(); // quick and dirty; prefer a prefab for real UI
            text.text = card.data.cardName;
            go.transform.SetParent(defenseListParent, false);
        }
    }

    private void OnBuyCard(CardData data)
    {
        if (activeEconomy.Spend(data.price))
        {
            activePlayerDeck.AddCard(data);
            RefreshUI(); // update gold and shop buttons
        }
        else
        {
            Debug.Log("Not enough gold!");
        }
    }
}
