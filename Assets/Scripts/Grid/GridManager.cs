using System.Collections.Generic;
using UnityEngine;
using static UnitModel;

public class GridManager: MonoBehaviour
{
    [SerializeField] private int width = 8;
    [SerializeField] private int height = 4;

    private GridCell[,] _cells;

    private void Awake()
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

    public void MoveUnit(Unit unit, Vector2Int newPos)
    {
        // 念のためチェック
        if (!CanPlace(newPos)) return;

        var oldPos = unit.GridPos;

        // 元のマスを空にする
        _cells[oldPos.x, oldPos.y].unit = null;

        // 新しいマスに置く
        _cells[newPos.x, newPos.y].unit = unit;

        // Unit側も更新
        unit.SetGridPos(newPos);
    }

    /// <summary>
    /// 移動できるかどうかをboolで判断する
    /// </summary>
    /// <param name="unit"></param>
    /// <param name="pos"></param>
    /// <returns></returns>
    public bool TryMoveUnit(Unit unit, Vector2Int pos)
    {
        if (!CanPlace(pos)) return false;

        MoveUnit(unit, pos);
        return true;
    }

    public IEnumerable<GridCell> AllCells
    {
        get
        {
            for (int x = 0; x < width; x++)
                for (int y = 0; y < height; y++)
                {
                    yield return _cells[x, y];
                }
        }
    }

    public bool CanPlace(Vector2Int pos)
    {
        if (!IsInRange(pos)) return false;

        return _cells[pos.x, pos.y].IsEmpty;
    }

    //盤面外かどうか
    private bool IsInRange(Vector2Int pos)
    {
        return pos.x >= 0 && pos.x < width &&
               pos.y >= 0 && pos.y < height;
    }


}
