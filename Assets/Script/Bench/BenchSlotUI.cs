using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

/// <summary>
/// 個々のスロットのUIを管理するクラス
/// </summary>
public class BenchSlotUI : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [SerializeField] private Image _unitIcon;
    [SerializeField] private GameObject _highlight;
    [SerializeField] private Canvas _canvas;

    private UnitInstance unit;
    private RectTransform rectTransform;
    private CanvasGroup canvasGroup;
    private Transform originalParent;
    private Vector2 originalAnchoredPosition;
    private int x;
    private int y;
    public UnitInstance Unit => unit;
    public int X => x;
    public int Y => y;

    public void Initialize(int x, int y)
    {
        this.x = x;
        this.y = y;
    }

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();

        if (canvasGroup == null)
        {
            canvasGroup = gameObject.AddComponent<CanvasGroup>();
        }
    }

    /// <summary>
    /// ユニットを配置する
    /// </summary>
    /// <param name="unit"></param>
    public void SetUnit(UnitInstance unit)
    {
        this.unit = unit;
        Debug.Log($"{this.unit.Data.name}を設置");

        _unitIcon.sprite = unit.Data.Icon;
        _unitIcon.enabled = true;
    }

    /// <summary>
    /// ユニットを撤去する
    /// </summary>
    public void Clear()
    {
        unit = null;

        _unitIcon.sprite = null;
        _unitIcon.enabled = false;
    }

    /// <summary>
    /// ユニットが選択されているかどうかを表示するハイライト
    /// </summary>
    /// <param name="selected"></param>
    public void SetSelected(bool selected)
    {
        _highlight.SetActive(selected);
    }

    public void SetCanvas(Canvas canvas) 
    {
        _canvas = canvas;
    }

    //ドラック開始
    public void OnBeginDrag(PointerEventData eventData)
    {
        if (unit == null)
        {
            return;
        }

        originalParent = transform.parent;
        originalAnchoredPosition = rectTransform.anchoredPosition;

        transform.SetParent(_canvas.transform, true);
        transform.SetAsLastSibling();

        canvasGroup.blocksRaycasts = false;
    }

    //ドラック中
    public void OnDrag(PointerEventData eventData)
    {
        if (unit == null)
        {
            return;
        }

        rectTransform.position = eventData.position;
    }

    //ドラック終了
    public void OnEndDrag(PointerEventData eventData)
    {
        if (unit == null)
        {
            return;
        }

        canvasGroup.blocksRaycasts = true;

        transform.SetParent(originalParent, false);
        rectTransform.anchoredPosition = originalAnchoredPosition;
    }

    /// <summary>
    /// 置いた時の演出
    /// </summary>
    public void PlayPlaceEffect()
    {
        // 光る演出、拡大縮小、パーティクルなど
    }

    /// <summary>
    /// 消した時の演出
    /// </summary>
    public void PlayRemoveEffect()
    {
        // 消える演出、縮小、パーティクルなど
    }
}