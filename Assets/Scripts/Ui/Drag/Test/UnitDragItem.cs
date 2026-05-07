using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class UnitDragItem : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    private Transform _originalParent;
    private CanvasGroup _canvasGroup;
    private bool _dropped;

    [SerializeField] private Image _image;
    [SerializeField] private UnitData unitData;

    public UnitData UnitData => unitData;

    void Awake()
    {
        _canvasGroup = GetComponent<CanvasGroup>();

        if (_image == null)
        {
            _image = GetComponent<Image>();
        }
    }

    public void SetUnitData(UnitData data)
    {
        unitData = data;

        if (_image != null && unitData != null)
        {
            _image.sprite = unitData.Icon;
            _image.enabled = unitData.Icon != null;
        }
    }

    // ドラッグ開始
    public void OnBeginDrag(PointerEventData eventData)
    {
        //元の位置を保存
        _originalParent = transform.parent;

        transform.SetParent(transform.root);

        _canvasGroup.blocksRaycasts = false; // 下に判定を通す
        _dropped = false;
    }

    // ドラッグ中
    public void OnDrag(PointerEventData eventData)
    {
        transform.position = eventData.position;
    }

    // ドラッグ終了
    public void OnEndDrag(PointerEventData eventData)
    {
        _canvasGroup.blocksRaycasts = true;

        // ドロップ失敗なら戻る
        if (!_dropped)
        {
            transform.SetParent(_originalParent);
            transform.localPosition = Vector3.zero;
        }
    }

    // スロット側から呼ばれる
    public void SetDropped()
    {
        _dropped = true;
    }

    public Transform GetOriginalParent()
    {
        return _originalParent;
    }
}