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

        benchManager.SwapUnit(draggedUI,x,y);

        // UIˆÚ“®
        draggedUI.transform.SetParent(transform, false);

        var rect = draggedUI.GetComponent<RectTransform>();
        rect.anchoredPosition = Vector2.zero;

        draggedUI.Initialize(x, y);
    }
}