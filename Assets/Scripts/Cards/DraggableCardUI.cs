using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DraggableCardUI : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public Card card;
    public Image image;

    private Transform originalParent;
    private Canvas canvas;

    private void Awake()
    {
        canvas = GetComponentInParent<Canvas>();
    }

    public void Setup(Card cardData)
    {
        card = cardData;
        image.sprite = card.data.artwork;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        originalParent = transform.parent;
        transform.SetParent(canvas.transform);
        image.raycastTarget = false;
        CardDragManager.Instance.BeginDrag(this);
    }

    public void OnDrag(PointerEventData eventData)
    {
        transform.position = Input.mousePosition;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        image.raycastTarget = true;
        transform.SetParent(originalParent);
        CardDragManager.Instance.EndDrag(this, eventData);
    }
}
