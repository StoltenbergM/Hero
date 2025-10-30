using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ShopCardUI : MonoBehaviour
{
    public Image artworkImage;
    public TMP_Text nameText;
    public TMP_Text priceText;
    public Button buyButton;

    private CardData cardData;
    private System.Action<CardData> onBuy;

    public void Setup(CardData data, System.Action<CardData> onBuyCallback, bool canBuy)
    {
        cardData = data;
        onBuy = onBuyCallback;

        if (artworkImage != null) artworkImage.sprite = cardData.artwork;
        if (nameText != null) nameText.text = cardData.cardName;
        if (priceText != null) priceText.text = cardData.price.ToString();

        buyButton.interactable = canBuy;
        buyButton.onClick.RemoveAllListeners();
        buyButton.onClick.AddListener(() => onBuy?.Invoke(cardData));
    }
}
