using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class CharacterIconView : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [SerializeField] private Image iconImage;
    [SerializeField] private Canvas canvas;

    private RectTransform rectTransform;
    private CanvasGroup canvasGroup;

    private Transform originalParent;
    private Vector2 originalAnchoredPosition;
    private bool dropped;

    public UnitInstance Unit { get; private set; }

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();

        if (canvasGroup == null)
        {
            canvasGroup = gameObject.AddComponent<CanvasGroup>();
        }
    }

    public void Initialize(UnitInstance unit)
    {
        Unit = unit;
        iconImage.sprite = unit.Data.Icon;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (Unit == null)
        {
            return;
        }

        dropped = false;

        originalParent = transform.parent;
        originalAnchoredPosition = rectTransform.anchoredPosition;

        transform.SetParent(canvas.transform, true);
        transform.SetAsLastSibling();

        canvasGroup.blocksRaycasts = false;

        DragData.SetUnit(Unit);

        Debug.Log($"ドラッグ開始: {Unit.Data.CharacterName}");
    }

    public void OnDrag(PointerEventData eventData)
    {
        rectTransform.position = eventData.position;
    }

    public void OnEndDrag(PointerEventData eventData)
    {

        Debug.Log($"ドラッグ終了: {Unit.Data.CharacterName}");
        canvasGroup.blocksRaycasts = true;
        DragData.Clear();

        if (!dropped)
        {
            transform.SetParent(originalParent, false);
            rectTransform.anchoredPosition = originalAnchoredPosition;
        }
    }

    public void DropTo(Transform slotParent)
    {
        dropped = true;

        transform.SetParent(slotParent, false);
        rectTransform.anchoredPosition = Vector2.zero;
    }
}