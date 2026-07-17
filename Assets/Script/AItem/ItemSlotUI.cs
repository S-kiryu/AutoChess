using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ItemSlotUI : MonoBehaviour,
    IBeginDragHandler,
    IDragHandler,
    IEndDragHandler
{
    [SerializeField] private Image icon;
    [SerializeField] private Canvas canvas;

    private ItemInstance item;
    private RectTransform rectTransform;
    private CanvasGroup canvasGroup;
    private Transform originalParent;
    private Vector2 originalAnchoredPosition;
    private bool dropped;
    private bool dragging;

    public ItemInstance Item => item;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();

        if (canvasGroup == null)
        {
            canvasGroup = gameObject.AddComponent<CanvasGroup>();
        }
    }

    public void SetItem(ItemInstance newItem)
    {
        item = newItem;

        if (item == null || item.Data == null)
        {
            icon.sprite = null;
            icon.enabled = false;
            return;
        }

        icon.sprite = item.Data.Icon;
        icon.enabled = true;
    }

    public void SetCanvas(Canvas newCanvas)
    {
        canvas = newCanvas;
    }

    public void SetDropped(bool value)
    {
        dropped = value;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        dragging = false;

        if (item == null || canvas == null)
        {
            return;
        }

        dragging = true;
        dropped = false;

        originalParent = transform.parent;
        originalAnchoredPosition = rectTransform.anchoredPosition;

        transform.SetParent(canvas.transform, true);
        transform.SetAsLastSibling();

        canvasGroup.blocksRaycasts = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (!dragging)
        {
            return;
        }

        rectTransform.position = eventData.position;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (!dragging)
        {
            return;
        }

        dragging = false;
        canvasGroup.blocksRaycasts = true;

        if (dropped)
        {
            return;
        }

        transform.SetParent(originalParent, false);
        rectTransform.anchoredPosition = originalAnchoredPosition;
    }
}