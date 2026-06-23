using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.UI.CanvasScaler;

public class BattleGridManager : MonoBehaviour
{
    public static BattleGridManager Instance { get; private set; }
    [SerializeField] private BattleGrid grid;
    [SerializeField] private int x;
    public int X => x;
    [SerializeField] private int y;
    public int Y => y;

    //配置できる行の数
    [SerializeField] private int Unitplace;
    //グリットの色
    [SerializeField] private Color color1;
    [SerializeField] private Color color2;
    [SerializeField] private Color color3;
    [SerializeField] private Color color4;

    [SerializeField] private GridLayoutGroup gridLayoutGroup;
    [SerializeField] private BenchManager benchManager;
    private BattleGrid[,] _battleGrid;
    private BattleGrid[,] _PlayerBattleGrid;
    private BattleGrid[,] _EnemyBattleGrid;

    //置いた時
    public event System.Action<UnitInstance, int, int> OnUnitPlaced;
    //撤去したとき
    public event System.Action<UnitInstance, int, int> OnUnitRemoved;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        _battleGrid = new BattleGrid[x,y];
        gridLayoutGroup.constraintCount = x;
        GenerateGrid();
    }

    //グリットを生成して色を変える
    private void GenerateGrid()
    {
        for (int i = 0; i < x; i++)
        {
            for (int j = 0; j < y; j++)
            {
                var battleGrid = Instantiate(grid, transform);

                battleGrid.name = $"BattleGrid_{i}_{j}";

                bool isEven = (i + j) % 2 == 0;

                //グリットに座標とマネージャーを持たせた
                Vector2Int vect = new Vector2Int(i,j);
                battleGrid.Initialize(i,j, benchManager);
                Image image = battleGrid.GetComponentInChildren<Image>();

                if (image != null)
                {
                    image.color = isEven ? color1 : color2;
                }

                _battleGrid[i, j] = battleGrid;
            }
        }

        _EnemyBattleGrid = new BattleGrid[Unitplace,y];
        // 左側 Unitplace 列を 敵のグリット にする
        for (int i = 0; i < Unitplace; i++)
        {
            for (int j = 0; j < y; j++)
            {
                _EnemyBattleGrid[i, j] = _battleGrid[i, j];
                Image image = _battleGrid[i, j].GetComponentInChildren<Image>();
                if (image != null)
                {
                    image.color = color3;
                }
            }
        }

        _PlayerBattleGrid = new BattleGrid[Unitplace, y];

        // 右側 Unitplace 列を プレイヤーのグリッド にする
        for (int i = x - Unitplace; i < x; i++)
        {
            for (int j = 0; j < y; j++)
            {
                int playerX = i - (x - Unitplace);

                _PlayerBattleGrid[playerX, j] = _battleGrid[i, j];

                Image image = _battleGrid[i, j].GetComponentInChildren<Image>();
                if (image != null)
                {
                    image.color = color4;
                }
            }
        }
    }

    public void MoveUnitFromGrid(int x,int y) 
    {
        benchManager.RemoveUnit(x, y);
    }

    public bool SetUnit(UnitInstance unit, BattleGrid[,] grid, int x, int y) 
    {
        Debug.Log("ユニットを配置");
        if (!IsInside(x, y))
        {
            Debug.Log("範囲外");
            return false;
        }

        // すでにユニットが配置されている場合は置けない
        if (grid[x, y].CurrentUnit != null)
        {
            Debug.Log("ユニットがもういる");
            return false;
        }

        
        grid[x, y].SetUnit(unit);
        OnUnitPlaced?.Invoke(unit, x, y);
        return true;
    }

    public bool RemoveUnit(int x, int y)
    {
        if (!IsInside(x, y))
        {
            return false;
        }

        UnitInstance unit = _PlayerBattleGrid[x, y].CurrentUnit;

        if (unit == null)
        {
            return false;
        }

        _PlayerBattleGrid[x, y].SetUnit(null);
        OnUnitRemoved?.Invoke(unit, x, y);
        return true;
    }

    public bool SwapUnit(BenchSlotUI draggedUI, int x, int y)
    {
        Debug.Log("Unitをセットを試す");
        draggedUI.SetDropped(true);

        int fromX = draggedUI.X;
        int fromY = draggedUI.Y;

        if (fromX == x && fromY == y) 
        {
            Debug.Log("同じところに落とされた");
            return false;
        } 

        var movingUnit = draggedUI.Unit;
        var targetUnit = GetUnit(x, y);

        RemoveUnit(fromX, fromY);

        if (targetUnit != null)
        {
            Debug.Log("入れ替える");
            RemoveUnit(x, y);
            SetUnit(targetUnit, _PlayerBattleGrid, fromX, fromY);
        }

        SetUnit(movingUnit, _PlayerBattleGrid, x, y);
        draggedUI.isBattle = true;

        return true;
    }

    public UnitInstance GetUnit(int x, int y)
    {
        if (!IsInside(x, y))
        {
            return null;
        }

        return _PlayerBattleGrid[x, y].CurrentUnit;
    }

    /// <summary>
    /// 範囲内かどうかをチェックする
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <returns></returns>
    private bool IsInside(int x, int y)
    {
        return x >= 0 && x < _battleGrid.GetLength(0)
            && y >= 0 && y < _battleGrid.GetLength(1);
    }

}
