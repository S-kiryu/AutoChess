using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

/// <summary>
/// ドラッグ可能なキャラクターアイコンのUIクラス
/// </summary>
public class CharacterIconView : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [SerializeField] private Image iconImage;
    [SerializeField] private Canvas canvas;

    private RectTransform rectTransform;
    private CanvasGroup canvasGroup;
    private Transform originalParent;
    private Transform homeParent;
    private Vector2 homeAnchoredPosition;
    private Vector2 originalAnchoredPosition;
    private FormationSlotView currentSlotView;
    private bool droppedToSlot;

    public UnitInstance Unit { get; private set; }
    public FormationSlotView PreviousSlotView { get; private set; }

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();

        if (canvasGroup == null)
        {
            canvasGroup = gameObject.AddComponent<CanvasGroup>();
        }

        if (canvas == null)
        {
            canvas = GetComponentInParent<Canvas>();
        }
    }

    public void Initialize(UnitInstance unit)
    {
        Unit = unit;
        iconImage.sprite = unit.Data.Icon;

        homeParent = transform.parent;
        homeAnchoredPosition = rectTransform.anchoredPosition;
        SetAssigned(false);
    }

    public void SetAssigned(bool assigned)
    {
        iconImage.color = assigned
            ? new Color(0.4f, 0.4f, 0.4f, 1f)
            : Color.white;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        droppedToSlot = false;
        originalParent = transform.parent;
        originalAnchoredPosition = rectTransform.anchoredPosition;

        if (currentSlotView != null)
        {
            PreviousSlotView = currentSlotView;
            currentSlotView.DetachIcon(this);
            currentSlotView = null;
            SetAssigned(false);
        }
        else
        {
            PreviousSlotView = null;
        }

        transform.SetParent(canvas.transform, true);
        transform.SetAsLastSibling();
        canvasGroup.blocksRaycasts = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        rectTransform.position = eventData.position;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        canvasGroup.blocksRaycasts = true;

        if (Unit == null)
        {
            return;
        }

        if (droppedToSlot)
        {
            return;
        }

        if (PreviousSlotView != null && PreviousSlotView.RestoreIcon(this))
        {
            SetToSlot(PreviousSlotView);
            PreviousSlotView = null;
            return;
        }

        ReturnHomePosition();
        PreviousSlotView = null;
    }

    public void Returnposition()
    {
        transform.SetParent(originalParent, false);
        rectTransform.anchoredPosition = originalAnchoredPosition;
    }

    public void ReturnHomePosition()
    {
        transform.SetParent(homeParent, false);
        rectTransform.anchoredPosition = homeAnchoredPosition;
        currentSlotView = null;
        SetAssigned(false);
    }

    public void SetToSlot(FormationSlotView slotView)
    {
        droppedToSlot = true;
        currentSlotView = slotView;

        transform.SetParent(slotView.transform, false);
        rectTransform.anchoredPosition = Vector2.zero;
        SetAssigned(true);
    }
}