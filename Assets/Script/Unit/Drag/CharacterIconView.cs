using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

/// <summary>
/// ドラック可能なキャラクターアイコンのUIクラス
/// </summary>
public class CharacterIconView : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [SerializeField] private Image iconImage;
    [SerializeField] private Canvas canvas;

    private RectTransform rectTransform;
    private CanvasGroup canvasGroup;
    private Transform originalParent;
    private Vector2 originalAnchoredPosition;
    private bool isAssigned;

    public UnitInstance Unit { get; private set; }

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
        SetAssigned(false);
    }

    //編成に配置されているキャラクターのアイコンをグレーアウトするためのメソッド
    public void SetAssigned(bool assigned)
    {
        isAssigned = assigned;
        iconImage.color = assigned ? new Color(0.4f, 0.4f, 0.4f, 1f) : Color.white;
    }

    //ドラック開始時の処理
    public void OnBeginDrag(PointerEventData eventData)
    {
        if (Unit == null || isAssigned || canvas == null)
        {
            return;
        }

        originalParent = transform.parent;
        originalAnchoredPosition = rectTransform.anchoredPosition;

        transform.SetParent(canvas.transform, true);
        transform.SetAsLastSibling();

        // ドラック中は他のUI要素の下に表示されるようにするため、Raycastを無効化
        canvasGroup.blocksRaycasts = false;
    }

    //ドラック中の処理
    public void OnDrag(PointerEventData eventData)
    {
        if (Unit == null || isAssigned)
        {
            return;
        }

        rectTransform.position = eventData.position;
    }

    //ドラック終了時の処理
    public void OnEndDrag(PointerEventData eventData)
    {
        if (Unit == null || isAssigned)
        {
            return;
        }

        //元に戻した
        canvasGroup.blocksRaycasts = true;
        transform.SetParent(originalParent, false);
        rectTransform.anchoredPosition = originalAnchoredPosition;
    }
}