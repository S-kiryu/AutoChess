using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

/// <summary>
/// ドラッグできるユニットUI。
/// ベンチと戦闘グリッドの両方で同じオブジェクトを使用する。
/// </summary>
public class BenchSlotUI : MonoBehaviour,
    IBeginDragHandler,
    IDragHandler,
    IEndDragHandler
{
    [SerializeField] private Image unitIcon;
    [SerializeField] private GameObject highlight;
    [SerializeField] private Canvas canvas;

    private UnitInstance unit;
    private RectTransform rectTransform;
    private CanvasGroup canvasGroup;

    private Transform originalParent;
    private Vector2 originalAnchoredPosition;

    private int x;
    private int y;
    private bool droppedOnSlot;
    private bool isDragging;

    public UnitInstance Unit => unit;
    public int X => x;
    public int Y => y;
    public UnitArea Area { get; private set; }
    public Transform OriginalParent => originalParent;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();

        if (canvasGroup == null)
        {
            canvasGroup = gameObject.AddComponent<CanvasGroup>();
        }
    }

    public void SetUnit(UnitInstance newUnit)
    {
        unit = newUnit;

        if (unit == null)
        {
            Clear();
            return;
        }

        unitIcon.sprite = unit.Data.Icon;
        unitIcon.enabled = true;
    }

    public void Clear()
    {
        unit = null;
        unitIcon.sprite = null;
        unitIcon.enabled = false;
    }

    public void SetCanvas(Canvas newCanvas)
    {
        canvas = newCanvas;
    }

    /// <summary>
    /// 現在所属している場所と座標を更新する。
    /// </summary>
    public void SetLocation(UnitArea area, int newX, int newY)
    {
        Area = area;
        x = newX;
        y = newY;
    }

    /// <summary>
    /// UIオブジェクトを破棄せず、指定スロットへ直接移動する。
    /// </summary>
    public void MoveTo(Transform destination)
    {
        transform.SetParent(destination, false);

        rectTransform.anchorMin = new Vector2(0.5f, 0.5f);
        rectTransform.anchorMax = new Vector2(0.5f, 0.5f);
        rectTransform.pivot = new Vector2(0.5f, 0.5f);
        rectTransform.anchoredPosition = Vector2.zero;
        rectTransform.localScale = Vector3.one;

        transform.SetAsLastSibling();
    }

    /// <summary>
    /// ユニットをアクティブにする
    /// </summary>
    public void SetSelected(bool selected)
    {
        if (highlight != null)
        {
            highlight.SetActive(selected);
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public void SetDropped(bool value)
    {
        droppedOnSlot = value;
    }

    /// <summary>
    /// ドラック始めに
    /// </summary>
    public void OnBeginDrag(PointerEventData eventData)
    {
        isDragging = false;

        if (GameLoopManager.Instance == null ||
            GameLoopManager.Instance.CurrentState != GameState.Preparation)
        {
            return;
        }

        if (unit == null || canvas == null)
        {
            return;
        }

        isDragging = true;
        droppedOnSlot = false;

        originalParent = transform.parent;
        originalAnchoredPosition = rectTransform.anchoredPosition;

        transform.SetParent(canvas.transform, true);
        transform.SetAsLastSibling();

        canvasGroup.blocksRaycasts = false;
    }

    /// <summary>
    /// ドラック中
    /// </summary>
    public void OnDrag(PointerEventData eventData)
    {
        if (!isDragging)
        {
            return;
        }

        rectTransform.position = eventData.position;
    }

    /// <summary>
    /// ドラック終了時
    /// </summary>
    public void OnEndDrag(PointerEventData eventData)
    {
        if (!isDragging)
        {
            return;
        }

        isDragging = false;

        if (unit == null)
        {
            return;
        }

        canvasGroup.blocksRaycasts = true;

        if (droppedOnSlot)
        {
            return;
        }

        transform.SetParent(originalParent, false);
        rectTransform.anchoredPosition = originalAnchoredPosition;
    }

    public void PlayPlaceEffect()
    {
        // 配置演出
    }

    public void PlayRemoveEffect()
    {
        // 売却演出
    }
}