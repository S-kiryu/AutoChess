using UnityEngine;
using UnityEngine.EventSystems;

// ドロップされたときに呼ばれる
public class FormationSlot : MonoBehaviour, IDropHandler
{
    public UnitData CurrentUnitData
    {
        get
        {
            var dragItem = GetCurrentDragItem();
            return dragItem != null ? dragItem.UnitData : null;
        }
    }

    private UnitDragItem GetCurrentDragItem()
    {
        return GetComponentInChildren<UnitDragItem>();
    }

    public void OnDrop(PointerEventData eventData)
    {
        var dragObj = eventData.pointerDrag;
        if (dragObj == null) return;

        var drag = dragObj.GetComponent<UnitDragItem>();
        if (drag == null) return;

        var originalParent = drag.GetOriginalParent();

        // このスロットにすでにいる UnitDragItem を取得
        var currentDrag = GetCurrentDragItem();

        // 同じスロットに戻しただけなら位置だけ戻して終了
        if (originalParent == transform)
        {
            dragObj.transform.SetParent(transform);
            dragObj.transform.localPosition = Vector3.zero;
            drag.SetDropped();
            return;
        }

        // 先に新しい方をこのスロットへ
        dragObj.transform.SetParent(transform);
        dragObj.transform.localPosition = Vector3.zero;

        // 既存のユニットがいたら元の場所へ戻す
        if (currentDrag != null && currentDrag.gameObject != dragObj)
        {
            currentDrag.transform.SetParent(originalParent);
            currentDrag.transform.localPosition = Vector3.zero;
        }

        drag.SetDropped();
    }
}
