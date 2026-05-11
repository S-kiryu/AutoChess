using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class UnitDragItem : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    private Transform _originalParent;
    private CanvasGroup _canvasGroup;
    private FormationSlot _currentSlot;

    private bool _dropped;

    [SerializeField] private Image _image;
    [SerializeField] private UnitData unitData;
    [SerializeField] private Transform _homeParent;

    public UnitData UnitData => unitData;

    void Awake()
    {
        _canvasGroup = GetComponent<CanvasGroup>();

        if (_image == null)
        {
            _image = GetComponent<Image>();
        }
    }

    //今いるスロットを設定
    public void SetCurrentSlot(FormationSlot slot)
    {
        _currentSlot = slot;
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

        var slot = _originalParent.GetComponent<FormationSlot>();
        if (slot != null)
        {
            slot.Clear(this);
            SetCurrentSlot(null);
        }

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

        if (!_dropped)
        {
            transform.SetParent(_homeParent);
            transform.localPosition = Vector3.zero;
            SetCurrentSlot(null);
        }

        _dropped = false;
    }

    // スロット側から呼ばれる
    public void SetDropped()
    {
        _dropped = true;
    }

    // 元の親を取得するためのメソッド
    public Transform GetOriginalParent()
    {
        return _originalParent;
    }

    // 元の親を設定するためのメソッド
    public void SetOriginalParent(Transform parent)
    {
        _originalParent = parent;
    }
}