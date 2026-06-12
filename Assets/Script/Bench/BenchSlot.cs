using UnityEngine;
using UnityEngine.EventSystems;

public class BenchSlot : MonoBehaviour, IDropHandler
{
    private int x;
    private int y;
    private BenchManager benchManager;

    public int X => x;
    public int Y => y;

    public void Initialize(int x, int y, BenchManager benchManager)
    {
        this.x = x;
        this.y = y;
        this.benchManager = benchManager;
    }

    public void OnDrop(PointerEventData eventData)
    {
        var draggedUI = eventData.pointerDrag?.GetComponent<BenchSlotUI>();

        if (draggedUI == null || draggedUI.Unit == null)
        {
            return;
        }

        // ★ ドロップ成功フラグ
        draggedUI.SetDropped(true);

        int fromX = draggedUI.X;
        int fromY = draggedUI.Y;

        if (fromX == x && fromY == y) return;

        var movingUnit = draggedUI.Unit;
        var targetUnit = benchManager.GetUnit(x, y);

        benchManager.RemoveUnit(fromX, fromY);

        if (targetUnit != null)
        {
            benchManager.RemoveUnit(x, y);
            benchManager.SetUnit(targetUnit, fromX, fromY);
        }

        benchManager.SetUnit(movingUnit, x, y);

        // UI移動
        draggedUI.transform.SetParent(transform, false);

        var rect = draggedUI.GetComponent<RectTransform>();
        rect.anchoredPosition = Vector2.zero;

        draggedUI.Initialize(x, y);
    }
}