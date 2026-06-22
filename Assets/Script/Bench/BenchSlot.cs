using UnityEngine;
using UnityEngine.EventSystems;

public class BenchSlot : MonoBehaviour, IDropHandler
{
    private int x;
    private int y;
    private BenchManager benchManager;
    private BattleGridManager battleGridManager;

    public int X => x;
    public int Y => y;

    public void Initialize(int x, int y, BenchManager benchManager)
    {
        this.x = x;
        this.y = y;
        this.benchManager = benchManager;
        battleGridManager = BattleGridManager.Instance;
    }

    public void OnDrop(PointerEventData eventData)
    {
        var draggedUI = eventData.pointerDrag?.GetComponent<BenchSlotUI>();

        if (draggedUI == null || draggedUI.Unit == null)
        {
            return;
        }
        //ユニットが戦闘グリットから来たのかを判定する
        if (draggedUI.isBattle) 
        {
            battleGridManager.RemoveUnit(draggedUI.X, draggedUI.Y);
        }

        benchManager.SwapUnit(draggedUI,x,y);

        // UI移動
        draggedUI.transform.SetParent(transform, false);

        var rect = draggedUI.GetComponent<RectTransform>();
        rect.anchoredPosition = Vector2.zero;

        draggedUI.Initialize(x, y);
    }
}