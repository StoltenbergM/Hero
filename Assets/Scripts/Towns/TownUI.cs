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
    public enum ShopCategory { Creatures, Magic, Upgrade, Boat }

    [Header("UI References")]
    public GameObject panel;

    public TMP_Text townNameText;
    public TMP_Text factionText;
    public TMP_Text levelText;
    public TMP_Text playerGoldText;

    public Transform shopListParent;
    public ShopCardUI shopCardPrefab;

    public Button btnCreatures;
    public Button btnMagic;
    public Button btnUpgrade;
    public Button btnBoat;

    [SerializeField] private PlayerDeck activePlayerDeck;
    [SerializeField] private TownController activeTown;

    public PlayerDeck ActivePlayerDeck => activePlayerDeck;
    public TownController ActiveTown => activeTown;

    private PlayerEconomy activeEconomy;

    private ShopCategory currentCategory;

    [Header("Player Deck UI")]
    public Transform playerSlotParent;
    public CardSlotUI slotPrefab;

    [Header("Town Defense UI")]
    public Transform townSlotParent;


    public void ShowTown(TownController town, PlayerDeck deck, PlayerEconomy economy)
    {
        // In case I forget to hook references to ShowTown
        if (town == null || deck == null || economy == null)
        {
            Debug.LogError("TownUI.ShowTown() was called with a NULL reference!");
            return;
        }
        
        activeTown = town;
        activePlayerDeck = deck;
        activeEconomy = economy;

        currentCategory = ShopCategory.Creatures;

        panel.SetActive(true);
        RefreshUI();

        btnCreatures.onClick.RemoveAllListeners();
        btnMagic.onClick.RemoveAllListeners();
        btnUpgrade.onClick.RemoveAllListeners();
        btnBoat.onClick.RemoveAllListeners();

        // Button listeners
        btnCreatures.onClick.AddListener(() => SwitchCategory(ShopCategory.Creatures));
        btnMagic.onClick.AddListener(() => SwitchCategory(ShopCategory.Magic));
        btnUpgrade.onClick.AddListener(() => SwitchCategory(ShopCategory.Upgrade));
        btnBoat.onClick.AddListener(() => SwitchCategory(ShopCategory.Boat));
    }

    public void CloseTown()
    {
        panel.SetActive(false);
        ClearShopList();
    }

    private void SwitchCategory(ShopCategory cat)
    {
        currentCategory = cat;
        RefreshUI();
    }

    private void RefreshUI()
    {
        if (activeTown == null) return;

        townNameText.text = activeTown.townData.townName;
        factionText.text = activeTown.townData.faction.ToString();
        levelText.text = $"Level {activeTown.currentLevel + 1}";
        playerGoldText.text = $"Gold: {activeEconomy.gold}";

        PopulateCurrentCategory();
        RefreshUISlots();
    }

    public void RefreshUISlots()
    {
        // Safe guards:
        if (activePlayerDeck == null || activeTown == null)
        {
            Debug.LogError("RefreshUISlots called without active references");
            return;
        }
        if (playerSlotParent == null || townSlotParent == null)
        {
            Debug.LogError("Slot parents not assigned in TownUI");
            return;
        }
        if (slotPrefab == null)
        {
            Debug.LogError("slotPrefab is missing!");
            return;
        }
        
        // Clear old slots
        foreach (Transform t in playerSlotParent) Destroy(t.gameObject);
        foreach (Transform t in townSlotParent) Destroy(t.gameObject);

        // PLAYER SLOTS
        for (int i = 0; i < activePlayerDeck.maxDeckSize; i++)
        {
            var slot = Instantiate(slotPrefab, playerSlotParent);
            slot.isPlayerSlot = true;
            slot.slotIndex = i;

            if (i < activePlayerDeck.ownedCards.Count)
            {
                Card card = activePlayerDeck.ownedCards[i];
                slot.SetCard(card);

                var ui = Instantiate(shopCardPrefab, slot.transform); // reuse display prefab
                ui.Setup(card.data, null, false); // no buy, just visual
            }
        }

        // TOWN SLOTS
        for (int i = 0; i < activeTown.maxDefenseSlots; i++)
        {
            var slot = Instantiate(slotPrefab, townSlotParent);
            slot.isPlayerSlot = false;
            slot.slotIndex = i;

            if (i < activeTown.defenseDeck.Count)
            {
                Card card = activeTown.defenseDeck[i];
                slot.SetCard(card);

                var ui = Instantiate(shopCardPrefab, slot.transform);
                ui.Setup(card.data, null, false);
            }
        }
    }

    private void PopulateCurrentCategory()
    {
        ClearShopList();

        switch (currentCategory)
        {
            case ShopCategory.Creatures: PopulateCreatures(); break;
            case ShopCategory.Magic: PopulateMagic(); break;
            case ShopCategory.Upgrade: PopulateUpgrade(); break;
            case ShopCategory.Boat: PopulateBoat(); break;
        }
    }

    private void ClearShopList()
    {
        for (int i = shopListParent.childCount - 1; i >= 0; i--)
            Destroy(shopListParent.GetChild(i).gameObject);
    }

    // -----------------------------
    // CATEGORY POPULATION METHODS
    // -----------------------------

    private void PopulateCreatures()
    {
        Debug.Log("Number of cards: " + activeTown.townData.creatureCards.Count);
        foreach (var card in activeTown.townData.creatureCards)
        {
            bool unlocked = activeTown.CanBuyCard(card);
            bool canAfford = activeEconomy.gold >= card.price;

            var item = Instantiate(shopCardPrefab, shopListParent);
            item.Setup(card, OnBuyCard, unlocked && canAfford);
            item.SetLocked(!unlocked);
        }
    }

    private void PopulateMagic()
    {
        PlayerProgress progress = activePlayerDeck.GetComponent<PlayerProgress>();

        foreach (var tier in activeTown.townData.magicTiers)
        {
            if (progress.magicLevel < tier.requiredMagicLevel) continue;  // locked tier

            foreach (var card in tier.magicCards)
            {
                var item = Instantiate(shopCardPrefab, shopListParent);
                bool canBuy = activeEconomy.gold >= card.price;

                item.Setup(card, OnBuyCard, canBuy); // Magic cards are bought same as creature cards
            }
        }
    }

    private void PopulateUpgrade()
    {
        bool canUpgrade = activeTown.CanUpgrade();

        var item = Instantiate(shopCardPrefab, shopListParent);
        item.SetupUpgrade(
            "Upgrade Town",
            activeTown.upgradePrice,
            canUpgrade,
            OnBuyUpgrade
        );
    }

    private void PopulateBoat()
    {
        PlayerProgress progress = activePlayerDeck.GetComponent<PlayerProgress>();
        bool canBuy = !progress.hasBoat;

        var item = Instantiate(shopCardPrefab, shopListParent);
        item.SetupBoat("Buy Boat", activeTown.boatPrice, canBuy, OnBuyBoat);
    }


    // -----------------------------
    // ACTIONS
    // -----------------------------

    private void OnBuyCard(CardData data)
    {
        if (activeEconomy.Spend(data.price))
        {
            // default → goes to town defense
            Card newCard = new Card(data);
            activeTown.defenseDeck.Add(newCard);

            RefreshUI();
        }
    }

    private void OnBuyUpgrade(int price)
    {
        if (activeEconomy.Spend(price))
        {
            activeTown.UpgradeTown();
            RefreshUI();
        }
    }

    private void OnBuyBoat(int price)
    {
        PlayerProgress progress = activePlayerDeck.GetComponent<PlayerProgress>();

        if (activeEconomy.Spend(price))
        {
            progress.hasBoat = true;
            RefreshUI();
        }
    }
}