using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 戦闘グリッドを生成し、プレイヤー側ユニットを管理する。
///
/// 画面上側：敵の配置エリア
/// 画面中央：配置不可エリア
/// 画面下側：プレイヤーの配置エリア
/// </summary>
public class BattleGridManager : MonoBehaviour
{
    public static BattleGridManager Instance { get; private set; }

    [Header("グリッド")]
    [SerializeField] private BattleGrid gridPrefab;
    [SerializeField] private GridLayoutGroup gridLayoutGroup;

    [Header("グリッド全体のサイズ")]
    [SerializeField] private int x = 8;
    [SerializeField] private int y = 8;

    [Header("上下それぞれの配置可能な行数")]
    [SerializeField] private int unitPlace = 3;

    [Header("通常グリッドの模様")]
    [SerializeField] private Color color1 = Color.white;
    [SerializeField] private Color color2 = Color.gray;

    [Header("配置エリアの色")]
    [SerializeField] private Color enemyAreaColor = Color.red;
    [SerializeField] private Color playerAreaColor = Color.blue;

    [SerializeField] private BenchManager benchManager;
    [SerializeField] private Transform battleUnitRoot;


    private BattleGrid[,] battleGrids;

    private BattleGrid[,] playerBattleGrids;

    private BattleGrid[,] enemyBattleGrids;

    public bool IsReady { get; private set; }
    public int X => x;
    public int Y => y;
    public int UnitPlace => unitPlace;
    public Transform BattleUnitRoot => battleUnitRoot;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    private void Start()
    {
        if (!ValidateSettings())
        {
            enabled = false;
            return;
        }

        battleGrids = new BattleGrid[x, y];

        gridLayoutGroup.constraint =
            GridLayoutGroup.Constraint.FixedColumnCount;

        gridLayoutGroup.constraintCount = x;

        GenerateGrid();
        SetEnemyArea();
        SetPlayerArea();

        IsReady = true;
    }


    /// <summary>
    /// 設定できるグリットが正しいのかを見る
    /// </summary>
    /// <returns></returns>
    private bool ValidateSettings()
    {
        if (gridPrefab == null)
        {
            Debug.LogError(
                "BattleGridManagerにGrid Prefabが設定されていません。",
                this);

            return false;
        }

        if (gridLayoutGroup == null)
        {
            Debug.LogError(
                "BattleGridManagerにGrid Layout Groupが設定されていません。",
                this);

            return false;
        }

        if (benchManager == null)
        {
            Debug.LogError(
                "BattleGridManagerにBench Managerが設定されていません。",
                this);

            return false;
        }

        if (x <= 0 || y <= 0)
        {
            Debug.LogError(
                "グリッドサイズは1以上にしてください。",
                this);

            return false;
        }

        if (unitPlace <= 0)
        {
            Debug.LogError(
                "Unit Placeは1以上にしてください。",
                this);

            return false;
        }

        if (unitPlace * 2 > y)
        {
            Debug.LogError(
                "Unit Placeが大きすぎます。" +
                "敵とプレイヤーの配置エリアが重なっています。",
                this);

            return false;
        }

        return true;
    }

    /// <summary>
    /// グリッドを上の行から下の行へ生成する。
    /// </summary>
    private void GenerateGrid()
    {
        // 縦座標を外側のループにする
        for (int gridY = 0; gridY < y; gridY++)
        {
            // 横座標を内側のループにする
            for (int gridX = 0; gridX < x; gridX++)
            {
                BattleGrid newGrid =
                    Instantiate(gridPrefab, transform);

                newGrid.name =
                    $"BattleGrid_{gridX}_{gridY}";

                newGrid.Initialize(
                    gridX,
                    gridY,
                    benchManager);

                bool isEven =
                    (gridX + gridY) % 2 == 0;

                newGrid.SetColor(
                    isEven ? color1 : color2);

                battleGrids[gridX, gridY] = newGrid;
            }
        }
    }

    /// <summary>
    /// 上側のunitPlace行を敵エリアにする。
    ///
    /// 全体座標：
    /// y = 0 ～ unitPlace - 1
    /// </summary>
    private void SetEnemyArea()
    {
        enemyBattleGrids =
            new BattleGrid[x, unitPlace];

        for (int enemyY = 0; enemyY < unitPlace; enemyY++)
        {
            for (int gridX = 0; gridX < x; gridX++)
            {
                BattleGrid targetGrid =
                    battleGrids[gridX, enemyY];

                enemyBattleGrids[gridX, enemyY] =
                    targetGrid;

                targetGrid.SetAsEnemyGrid(gridX, enemyY);
                targetGrid.SetColor(enemyAreaColor);
            }
        }
    }

    /// <summary>
    /// 下側のunitPlace行をプレイヤーエリアにする。
    ///
    /// 全体座標：
    /// y - unitPlace ～ y - 1
    ///
    /// プレイヤーエリア内のローカル座標：
    /// 0 ～ unitPlace - 1
    /// </summary>
    private void SetPlayerArea()
    {
        playerBattleGrids =
            new BattleGrid[x, unitPlace];

        int playerStartY = y - unitPlace;

        for (int worldY = playerStartY; worldY < y; worldY++)
        {
            int playerY = worldY - playerStartY;

            for (int gridX = 0; gridX < x; gridX++)
            {
                BattleGrid targetGrid =
                    battleGrids[gridX, worldY];

                playerBattleGrids[gridX, playerY] =
                    targetGrid;

                /*
                 * BattleGridが保持する座標を、
                 * プレイヤーエリア内の座標へ変更する。
                 */
                targetGrid.SetAsPlayerGrid(
                    gridX,
                    playerY);

                targetGrid.SetColor(playerAreaColor);
            }
        }
    }

    public UnitInstance GetUnit(
        int gridX,
        int gridY)
    {
        if (!IsInsidePlayerGrid(gridX, gridY))
        {
            return null;
        }

        return playerBattleGrids[gridX, gridY].CurrentUnit;
    }

    /// <summary>
    /// 戦闘グリッドからユニットデータだけ取り出す。
    /// </summary>
    public UnitInstance TakeUnit(
        int gridX,
        int gridY)
    {
        if (!IsInsidePlayerGrid(gridX, gridY))
        {
            return null;
        }

        BattleGrid targetGrid =
            playerBattleGrids[gridX, gridY];

        UnitInstance unit = targetGrid.CurrentUnit;

        if (unit == null)
        {
            return null;
        }

        targetGrid.SetUnit(null);
        return unit;
    }

    /// <summary>
    /// 戦闘グリッドへユニットデータを配置する。
    /// </summary>
    public bool PutUnit(
        UnitInstance unit,
        int gridX,
        int gridY)
    {
        if (unit == null)
        {
            return false;
        }

        if (!IsInsidePlayerGrid(gridX, gridY))
        {
            return false;
        }

        BattleGrid targetGrid =
            playerBattleGrids[gridX, gridY];

        if (targetGrid.CurrentUnit != null)
        {
            return false;
        }

        targetGrid.SetUnit(unit);
        return true;
    }

    /// <summary>
    /// プレイヤー戦闘グリッド内のユニットを交換する。
    /// </summary>
    public bool SwapUnits(
        int fromX,
        int fromY,
        int toX,
        int toY)
    {
        if (!IsInsidePlayerGrid(fromX, fromY) ||
            !IsInsidePlayerGrid(toX, toY))
        {
            return false;
        }

        if (fromX == toX && fromY == toY)
        {
            return false;
        }

        BattleGrid fromGrid =
            playerBattleGrids[fromX, fromY];

        BattleGrid toGrid =
            playerBattleGrids[toX, toY];

        UnitInstance movingUnit =
            fromGrid.CurrentUnit;

        if (movingUnit == null)
        {
            return false;
        }

        UnitInstance targetUnit =
            toGrid.CurrentUnit;

        fromGrid.SetUnit(targetUnit);
        toGrid.SetUnit(movingUnit);

        return true;
    }

    //エネミーを取得する
    public Vector3 GetEnemyWorldPosition(int gridX, int gridY)
    {
        if (!IsInsideEnemyGrid(gridX, gridY))
        {
            return Vector3.zero;
        }

        return enemyBattleGrids[gridX, gridY].transform.position;
    }

    //エネミーの範囲
    public bool IsInsideEnemyGrid(int gridX, int gridY)
    {
        if (enemyBattleGrids == null)
        {
            return false;
        }

        return gridX >= 0 &&
               gridX < enemyBattleGrids.GetLength(0) &&
               gridY >= 0 &&
               gridY < enemyBattleGrids.GetLength(1);
    }

    public BattleGrid GetEnemyGrid(int gridX, int gridY)
    {
        if (!IsInsideEnemyGrid(gridX, gridY))
        {
            return null;
        }

        return enemyBattleGrids[gridX, gridY];
    }

    public bool IsInsidePlayerGrid(
        int gridX,
        int gridY)
    {
        if (playerBattleGrids == null)
        {
            return false;
        }

        return gridX >= 0 &&
               gridX < playerBattleGrids.GetLength(0) &&
               gridY >= 0 &&
               gridY < playerBattleGrids.GetLength(1);
    }

    /// <summary>
    /// 
    /// </summary>
    public void RegisterPlayerUnitsForBattle()
    {
        if (playerBattleGrids == null)
        {
            Debug.LogWarning("playerBattleGrids がありません。");
            return;
        }

        for (int y = 0; y < playerBattleGrids.GetLength(1); y++)
        {
            for (int x = 0; x < playerBattleGrids.GetLength(0); x++)
            {
                BattleGrid grid = playerBattleGrids[x, y];

                BenchSlotUI unitUI =
                    grid.GetComponentInChildren<BenchSlotUI>(true);

                if (unitUI == null)
                {
                    Debug.Log($"[{x}, {y}] BenchSlotUIなし");
                    continue;
                }

                if (unitUI.Unit == null)
                {
                    Debug.Log($"[{x}, {y}] Unitなし");
                    continue;
                }

                BattleUnitBase battleUnit =
                    unitUI.GetComponent<BattleUnitBase>();

                if (battleUnit == null)
                {
                    Debug.LogWarning("BenchSlotUIにBattleUnitBaseが付いていません。", unitUI);
                    continue;
                }

                battleUnit.transform.SetParent(battleUnitRoot, true);
                battleUnit.SetCurrentGrid(grid);

                Debug.Log($"味方登録: {unitUI.Unit.Data.name} [{x}, {y}]");
                battleUnit.Initialize(unitUI.Unit, BattleTeam.Player);
            }
        }
    }
}