using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// ベンチスロットへのドロップを処理する。
/// </summary>
public class BenchSlot : MonoBehaviour, IDropHandler
{
    private int x;
    private int y;

    private BenchManager benchManager;
    private BattleGridManager battleGridManager;

    public int X => x;
    public int Y => y;

    public void Initialize(
        int newX,
        int newY,
        BenchManager manager)
    {
        x = newX;
        y = newY;

        benchManager = manager;
        battleGridManager = BattleGridManager.Instance;
    }

    public void OnDrop(PointerEventData eventData)
    {
        if (GameLoopManager.Instance == null ||
            GameLoopManager.Instance.CurrentState != GameState.Preparation)
        {
            return;
        }

        BenchSlotUI draggedUI =
            eventData.pointerDrag?.GetComponent<BenchSlotUI>();

        if (draggedUI == null || draggedUI.Unit == null)
        {
            return;
        }

        bool success;

        if (draggedUI.Area == UnitArea.Bench)
        {
            success = MoveInsideBench(draggedUI);
        }
        else
        {
            success = MoveFromBattle(draggedUI);
        }

        if (success)
        {
            draggedUI.SetDropped(true);
        }
    }

    /// <summary>
    /// ベンチ内での移動・交換。
    /// </summary>
    private bool MoveInsideBench(BenchSlotUI draggedUI)
    {
        int fromX = draggedUI.X;
        int fromY = draggedUI.Y;

        if (fromX == x && fromY == y)
        {
            return false;
        }

        Transform sourceSlot = draggedUI.OriginalParent;

        BenchSlotUI targetUI =
            GetComponentInChildren<BenchSlotUI>(true);

        bool success = benchManager.SwapUnits(
            fromX,
            fromY,
            x,
            y);

        if (!success)
        {
            return false;
        }

        if (targetUI != null && targetUI != draggedUI)
        {
            targetUI.MoveTo(sourceSlot);
            targetUI.SetLocation(
                UnitArea.Bench,
                fromX,
                fromY);
        }

        draggedUI.MoveTo(transform);
        draggedUI.SetLocation(UnitArea.Bench, x, y);

        return true;
    }

    /// <summary>
    /// 戦闘グリッドからベンチへの移動・交換。
    /// </summary>
    private bool MoveFromBattle(BenchSlotUI draggedUI)
    {
        if (battleGridManager == null)
        {
            battleGridManager = BattleGridManager.Instance;
        }

        int battleX = draggedUI.X;
        int battleY = draggedUI.Y;

        Transform sourceGrid = draggedUI.OriginalParent;

        UnitInstance movingUnit =
            battleGridManager.GetUnit(battleX, battleY);

        if (movingUnit == null)
        {
            return false;
        }

        UnitInstance targetUnit =
            benchManager.GetUnit(x, y);

        BenchSlotUI targetUI =
            GetComponentInChildren<BenchSlotUI>(true);

        battleGridManager.TakeUnit(battleX, battleY);

        if (targetUnit != null)
        {
            benchManager.TakeUnit(x, y);
        }

        if (!benchManager.PutUnit(movingUnit, x, y))
        {
            battleGridManager.PutUnit(
                movingUnit,
                battleX,
                battleY);

            if (targetUnit != null)
            {
                benchManager.PutUnit(targetUnit, x, y);
            }

            return false;
        }

        if (targetUnit != null)
        {
            battleGridManager.PutUnit(
                targetUnit,
                battleX,
                battleY);

            targetUI.MoveTo(sourceGrid);
            targetUI.SetLocation(
                UnitArea.Battle,
                battleX,
                battleY);
        }

        draggedUI.MoveTo(transform);
        draggedUI.SetLocation(UnitArea.Bench, x, y);

        return true;
    }
}