using UnityEngine;

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

    /// <summary>
    /// ˆÚ“®‚Å‚«‚é‚©‚Ç‚¤‚©‚ðbool‚Å”»’f‚·‚é
    /// </summary>
    /// <param name="unit"></param>
    /// <param name="pos"></param>
    /// <returns></returns>
    public bool TryMoveUnit(Unit unit, Vector2Int pos) 
    {
        //ˆÚ“®æ‚ª”Õ–ÊŠO‚È‚çŽ¸”s
        if (!IsInRange(pos))
            return false;

        var cell = _cells[pos.x, pos.y];
        if (!cell.IsEmpty) return false;

        cell.unit = unit;
        unit.SetGridPos(pos);

        return true;
    }

    //”Õ–ÊŠO‚©‚Ç‚¤‚©
    private bool IsInRange(Vector2Int pos)
    {
        return pos.x >= 0 && pos.x < width &&
               pos.y >= 0 && pos.y < height;
    }

}
