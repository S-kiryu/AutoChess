using UnityEngine;
using UnityEngine.EventSystems;

//ドロップされたときに呼ばれる
public class SimpleSlot : MonoBehaviour, IDropHandler
{
    public void OnDrop(PointerEventData eventData)
    {
        // ドロップされたオブジェクトを取得
        var dragObj = eventData.pointerDrag;
        if (dragObj == null) return;

        var drag = dragObj.GetComponent<SimpleDrag>();
        if (drag == null) return;

        Transform fromParent = dragObj.transform.parent;

        // すでに子オブジェクトがある場合は入れ替える
        Transform current = transform.childCount > 0 ? transform.GetChild(0) : null;

        // ドロップされたオブジェクトをこのスロットの子にする
        dragObj.transform.SetParent(transform);
        dragObj.transform.localPosition = Vector3.zero;

        // もし入れ替えがあった場合は、元の位置に戻す
        if (current != null)
        {
            var currentDrag = current.GetComponent<SimpleDrag>();

            // 元の親に戻す
            current.SetParent(currentDrag.GetOriginalParent());
            current.localPosition = Vector3.zero;
        }

        drag.SetDropped();
    }
}