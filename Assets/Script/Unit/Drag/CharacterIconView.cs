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
    private Transform originalParent;// ドラック開始時の親オブジェクトを保存するための変数
    private Transform setParent;// ドラック終了時に配置する親オブジェクトを保存するための変数
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

    /// <summary>
    /// 編成に配置されているかを確認して色を変える
    /// </summary>
    /// <param name="assigned"></param>
    public void SetAssigned(bool assigned)
    {
        isAssigned = assigned;
        if (assigned)
        {
            iconImage.color = new Color(0.4f, 0.4f, 0.4f, 1f);
        }
        else 
        {
            iconImage.color = Color.white;
        }
    }

    /// <summary>
    /// ドラック開始時の処理
    /// </summary>
    /// <param name="eventData"></param>
    public void OnBeginDrag(PointerEventData eventData)
    {
        //if (Unit == null || isAssigned || canvas == null)
        //{
        //    return;
        //}

        // ドラック開始時に親をCanvasに変更して、アイコンが他のUI要素の上に表示されるようにする
        originalParent = transform.parent;

        // ドラック開始時の位置を保存
        originalAnchoredPosition = rectTransform.anchoredPosition;

        transform.SetParent(canvas.transform, true);
        transform.SetAsLastSibling();

        // ドラック中は他のUI要素の下に表示されるようにするため、Raycastを無効化
        canvasGroup.blocksRaycasts = false;
    }

    /// <summary>
    /// ドラック中の処理
    /// </summary>
    /// <param name="eventData"></param>
    public void OnDrag(PointerEventData eventData)
    {
        //if (Unit == null || isAssigned)
        //{
        //    return;
        //}

        rectTransform.position = eventData.position;
    }

    /// <summary>
    /// ドラック終了時の処理
    /// </summary>
    /// <param name="eventData"></param>
    public void OnEndDrag(PointerEventData eventData)
    {
        //if (Unit == null || isAssigned)
        //{
        //    return;
        //}
    
        //キャストを有効に戻した
        canvasGroup.blocksRaycasts = true;
        if (isAssigned)
        {

        }
        else 
            Returnposition();
    }

    public void Returnposition() 
    {
        // ドラック終了時に元の親に戻す
        transform.SetParent(originalParent, false);
        //元の位置に戻す
        rectTransform.anchoredPosition = originalAnchoredPosition;
    }
}