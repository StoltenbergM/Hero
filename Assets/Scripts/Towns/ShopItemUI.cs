using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ShopItemUI : MonoBehaviour
{
    public TMP_Text nameText;
    public TMP_Text priceText;
    public Button buyButton;

    private CardData cardData;
    private TownUI townUI;

    public void Setup(CardData data, TownUI ui)
    {
        cardData = data;
        townUI = ui;

        nameText.text = data.cardName;
        priceText.text = $"{data.price} Gold";

        buyButton.onClick.RemoveAllListeners();
        buyButton.onClick.AddListener(() => townUI.TryBuyCard(data));
    }
}
