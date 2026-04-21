using UnityEngine;

public class GridManager: MonoBehaviour
{
    [SerializeField] private int width = 8;
    [SerializeField] private int height = 4;
    [SerializeField] private GameManager gameManager;
    public int Width => width;
    public int Height => height;

    private GridCell[,] _cells;

    private void Awake()
    {
        GenerateGrid();
    }


    /// <summary>
    /// 移動できるかどうかをboolで判断する
    /// </summary>
    /// <param name="unit"></param>
    /// <param name="pos"></param>
    /// <returns></returns>
    public bool TryMoveUnit(UnitModel unit, Vector2Int pos) 
    {
        if (!IsInRange(pos))
            return false;

        var targetCell = _cells[pos.x, pos.y];
        if (!targetCell.IsEmpty) return false;

        var oldPos = unit.GridPos;
        var oldCell = _cells[oldPos.x, oldPos.y];

        // 元のマスを空にする
        oldCell.unit = null;

        // 新しいマスに配置
        targetCell.unit = unit;

        // Unit側更新
        unit.SetGridPos(pos);

        return true;
    }

    //盤面外かどうか
    private bool IsInRange(Vector2Int pos)
    {
        return pos.x >= 0 && pos.x < width &&
               pos.y >= 0 && pos.y < height;
    }

    private void GenerateGrid() 
    {
        _cells = new GridCell[width, height];

        for (int x = 0; x < width; x++)
            for (int y = 0; y < height; y++)
            {
                _cells[x, y] = new GridCell
                {
                    pos = new Vector2Int(x, y)
                };
            }
    }
}
