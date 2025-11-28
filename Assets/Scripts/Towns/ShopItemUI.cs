using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class ShopCardUI : MonoBehaviour
{
    [Header("UI")]
    public TMP_Text titleText;
    public TMP_Text priceText;
    public Button buyButton;

    // Creature Card Mode
    private CardData cardData;
    private Action<CardData> onBuyCard;

    // Magic Mode
    private Action<int, int> onBuyMagic;
    private int magicLevel;
    private int magicPrice;

    // Upgrade Mode
    private Action<int> onBuyUpgrade;
    private int upgradePrice;

    // Boat Mode
    private Action<int> onBuyBoat;
    private int boatPrice;

    // -------------------------------
    // 🟢 CREATURE CARD SETUP
    // -------------------------------
    public void Setup(CardData data, Action<CardData> buyCallback, bool available)
    {
        cardData = data;
        onBuyCard = buyCallback;

        titleText.text = data.cardName;
        priceText.text = data.price.ToString();
        buyButton.interactable = available;

        buyButton.onClick.AddListener(() => onBuyCard?.Invoke(cardData));
    }

    // -------------------------------
    // 🔵 MAGIC UPGRADE SETUP
    // -------------------------------
    public void SetupMagic(string name, int level, int price, Action<int,int> callback, bool unlocked)
    {
        titleText.text = name;
        priceText.text = price.ToString();

        magicLevel = level;
        magicPrice = price;
        onBuyMagic = callback;

        buyButton.interactable = !unlocked;
        buyButton.onClick.AddListener(() => onBuyMagic?.Invoke(magicLevel, magicPrice));
    }

    // -------------------------------
    // 🟣 TOWN UPGRADE SETUP
    // -------------------------------
    public void SetupUpgrade(string name, int price, bool available, Action<int> callback)
    {
        titleText.text = name;
        priceText.text = price.ToString();

        upgradePrice = price;
        onBuyUpgrade = callback;
        buyButton.interactable = available;

        buyButton.onClick.AddListener(() => onBuyUpgrade?.Invoke(upgradePrice));
    }

    // -------------------------------
    // 🟡 BOAT PURCHASE SETUP
    // -------------------------------
    public void SetupBoat(string name, int price, bool available, Action<int> callback)
    {
        titleText.text = name;
        priceText.text = price.ToString();

        boatPrice = price;
        onBuyBoat = callback;
        buyButton.interactable = available;

        buyButton.onClick.AddListener(() => onBuyBoat?.Invoke(boatPrice));
    }
}
