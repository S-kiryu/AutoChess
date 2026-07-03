using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

/// <summary>
/// 戦闘グリッドの1マス。
/// ユニットと背景用Imageを管理する。
/// </summary>
public class BattleGrid : MonoBehaviour, IDropHandler
{
    [Header("グリッド背景")]
    [SerializeField] private Image backgroundImage;

    private int x;
    private int y;
    private int boardX;
    private int boardY;
    private BattleUnitBase currentBattleUnit;
    private BattleUnitBase movingUnit;

    private bool isPlayerGrid;
    private bool isEnemyGrid;
    public bool HasMovingUnit => movingUnit != null;
    public bool IsEnterBlocked => currentBattleUnit != null || movingUnit != null;

    private BenchManager benchManager;
    private BattleGridManager battleGridManager;

    private UnitInstance unitInstance;

    public UnitInstance CurrentUnit => unitInstance;

    public int X => x;
    public int Y => y;
    public int BoardX => boardX;
    public int BoardY => boardY;
    public BattleUnitBase CurrentBattleUnit => currentBattleUnit;
    public bool HasBattleUnit => currentBattleUnit != null;


    public bool IsPlayerGrid => isPlayerGrid;
    public bool IsEnemyGrid => isEnemyGrid;

    public void Initialize(int gridX, int gridY, BenchManager manager)
    {
        boardX = gridX;
        boardY = gridY;

        x = gridX;
        y = gridY;

        benchManager = manager;
        battleGridManager = BattleGridManager.Instance;

        isPlayerGrid = false;
        isEnemyGrid = false;

        if (backgroundImage == null)
        {
            backgroundImage = GetComponent<Image>();
        }
    }

    public bool TryLockForMove(BattleUnitBase unit)
    {
        if (unit == null)
        {
            return false;
        }

        if (currentBattleUnit != null || movingUnit != null)
        {
            return false;
        }

        movingUnit = unit;
        return true;
    }

    public void ClearMoveLock(BattleUnitBase unit)
    {
        if (movingUnit == unit)
        {
            movingUnit = null;
        }
    }

    /// <summary>
    /// プレイヤーが配置できる下側グリッドに設定する。
    /// 座標はプレイヤーエリア内のローカル座標になる。
    /// </summary>
    public void SetAsPlayerGrid(int playerX,int playerY)
    {
        x = playerX;
        y = playerY;

        isPlayerGrid = true;
        isEnemyGrid = false;
    }

    /// <summary>
    /// 敵側の上側グリッドに設定する。
    /// </summary>
    public void SetAsEnemyGrid(int playerX, int playerY)
    {
        x = playerX;
        y = playerY;

        isPlayerGrid = false;
        isEnemyGrid = true;
    }


    /// <summary>
    /// 色を設定する
    /// </summary>
    /// <param name="color"></param>
    public void SetColor(Color color)
    {
        if (backgroundImage != null)
        {
            backgroundImage.color = color;
        }
    }

    public void SetUnit(UnitInstance unit)
    {
        unitInstance = unit;
    }

    /// <summary>
    /// ドロップされたときの処理
    /// </summary>
    /// <param name="eventData"></param>
    public void OnDrop(PointerEventData eventData)
    {
        // プレイヤー配置エリア以外には置けない
        if (!isPlayerGrid)
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

        if (draggedUI.Area == UnitArea.Battle)
        {
            success = MoveInsideBattle(draggedUI);
        }
        else
        {
            success = MoveFromBench(draggedUI);
        }

        if (success)
        {
            draggedUI.SetDropped(true);
        }
    }

    public void SetBattleUnit(BattleUnitBase unit)
    {
        currentBattleUnit = unit;
    }

    public void ClearBattleUnit(BattleUnitBase unit)
    {
        if (currentBattleUnit == unit)
        {
            currentBattleUnit = null;
        }
    }

    /// <summary>
    /// プレイヤー戦闘グリッド内で移動または交換する。
    /// </summary>
    private bool MoveInsideBattle(BenchSlotUI draggedUI)
    {
        int fromX = draggedUI.X;
        int fromY = draggedUI.Y;

        if (fromX == x && fromY == y)
        {
            return false;
        }

        Transform sourceGrid = draggedUI.OriginalParent;

        BenchSlotUI targetUI =
            GetComponentInChildren<BenchSlotUI>(true);

        bool success = battleGridManager.SwapUnits(
            fromX,
            fromY,
            x,
            y);

        if (!success)
        {
            return false;
        }

        // 移動先に別ユニットがいた場合は元の場所へ移動
        if (targetUI != null && targetUI != draggedUI)
        {
            targetUI.MoveTo(sourceGrid);

            targetUI.SetLocation(
                UnitArea.Battle,
                fromX,
                fromY);
        }

        draggedUI.MoveTo(transform);
        draggedUI.SetLocation(UnitArea.Battle, x, y);

        return true;
    }

    /// <summary>
    /// ベンチからプレイヤー戦闘グリッドへ移動または交換する。
    /// </summary>
    private bool MoveFromBench(BenchSlotUI draggedUI)
    {
        int benchX = draggedUI.X;
        int benchY = draggedUI.Y;

        Transform sourceSlot = draggedUI.OriginalParent;

        UnitInstance movingUnit =
            benchManager.GetUnit(benchX, benchY);

        if (movingUnit == null)
        {
            return false;
        }

        UnitInstance targetUnit =
            battleGridManager.GetUnit(x, y);

        BenchSlotUI targetUI =
            GetComponentInChildren<BenchSlotUI>(true);

        // ベンチからデータだけ取り出す
        benchManager.TakeUnit(benchX, benchY);

        // 移動先にユニットがいた場合はデータだけ取り出す
        if (targetUnit != null)
        {
            battleGridManager.TakeUnit(x, y);
        }

        // 移動ユニットを戦闘グリッドへ配置
        if (!battleGridManager.PutUnit(movingUnit, x, y))
        {
            // 失敗した場合は元に戻す
            benchManager.PutUnit(
                movingUnit,
                benchX,
                benchY);

            if (targetUnit != null)
            {
                battleGridManager.PutUnit(
                    targetUnit,
                    x,
                    y);
            }

            return false;
        }

        // 移動先にいたユニットをベンチへ移動
        if (targetUnit != null)
        {
            if (!benchManager.PutUnit(
                    targetUnit,
                    benchX,
                    benchY))
            {
                // 万一戻せなかった場合は全データを復元
                battleGridManager.TakeUnit(x, y);

                battleGridManager.PutUnit(
                    targetUnit,
                    x,
                    y);

                benchManager.PutUnit(
                    movingUnit,
                    benchX,
                    benchY);

                return false;
            }

            if (targetUI != null)
            {
                targetUI.MoveTo(sourceSlot);

                targetUI.SetLocation(
                    UnitArea.Bench,
                    benchX,
                    benchY);
            }
        }

        draggedUI.MoveTo(transform);
        draggedUI.SetLocation(UnitArea.Battle, x, y);

        return true;
    }
}