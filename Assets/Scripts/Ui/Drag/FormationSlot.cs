using UnityEngine;
using UnityEngine.EventSystems;

// ドロップされたときに呼ばれる
public class FormationSlot : MonoBehaviour, IDropHandler
{
    private UnitDragItem _current;

    private void Awake()
    {
        _current = FindCurrentChild();
        if (_current != null)
        {
            _current.SetCurrentSlot(this);
        }
    }

    // 現在このスロットにいるユニットデータを取得
    public UnitData CurrentUnitData
    {
        get
        {
            var dragItem = GetCurrentDragItem();
            return dragItem != null ? dragItem.UnitData : null;
        }
    }

    public void Clear()
    {
        _current = null;
    }

    public void Clear(UnitDragItem item)
    {
        if (_current == item)
        {
            _current = null;
        }
    }

    private UnitDragItem GetCurrentDragItem()
    {
        if (_current != null && _current.transform.parent == transform)
        {
            return _current;
        }

        _current = FindCurrentChild();
        if (_current != null)
        {
            _current.SetCurrentSlot(this);
        }

        return _current;
    }

    private UnitDragItem FindCurrentChild()
    {
        foreach (Transform child in transform)
        {
            var dragItem = child.GetComponent<UnitDragItem>();
            if (dragItem != null)
            {
                return dragItem;
            }
        }

        return null;
    }

    private void Place(UnitDragItem drag)
    {
        drag.transform.SetParent(transform);
        drag.transform.localPosition = Vector3.zero;
        _current = drag;
        drag.SetCurrentSlot(this);
    }

    // ドロップされたときの処理
    public void OnDrop(PointerEventData eventData)
    {
        var dragObj = eventData.pointerDrag;
        if (dragObj == null) return;

        var drag = dragObj.GetComponent<UnitDragItem>();
        if (drag == null) return;

        var originalParent = drag.GetOriginalParent();
        var currentDrag = GetCurrentDragItem();

        // 同じ場所なら何もしない
        if (originalParent == transform)
        {
            Place(drag);
            drag.SetDropped();
            return;
        }

        // 既にいたやつを元の場所へ
        if (currentDrag != null && currentDrag != drag)
        {
            currentDrag.transform.SetParent(originalParent);
            currentDrag.transform.localPosition = Vector3.zero;

            var originalSlot = originalParent != null ? originalParent.GetComponent<FormationSlot>() : null;
            if (originalSlot != null)
            {
                originalSlot.Place(currentDrag);
            }
            else
            {
                currentDrag.SetCurrentSlot(null);
            }
        }

        // 新しいやつを配置
        Place(drag);
        drag.SetDropped();
    }
}
