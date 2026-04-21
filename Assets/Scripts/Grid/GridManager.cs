<<<<<<< HEAD
ï»¿using UnityEngine;
=======
using System.Collections.Generic;
using UnityEngine;
using static UnitModel;
>>>>>>> a074e9d56bed1d9b257ee15a8f711283f6fb7015

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
    /// ç§»å‹•ã§ãã‚‹ã‹ã©ã†ã‹ã‚’boolã§åˆ¤æ–­ã™ã‚‹
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

        // å…ƒã®ãƒã‚¹ã‚’ç©ºã«ã™ã‚‹
        oldCell.unit = null;

        // æ–°ã—ã„ãƒã‚¹ã«é…ç½®
        targetCell.unit = unit;

        // Unitå´æ›´æ–°
        unit.SetGridPos(pos);

        return true;
    }

    //ç›¤é¢å¤–ã‹ã©ã†ã‹
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
<<<<<<< HEAD
=======

    public void MoveUnit(Unit unit, Vector2Int newPos)
    {
        // ”O‚Ì‚½‚ßƒ`ƒFƒbƒN
        if (!CanPlace(newPos)) return;

        var oldPos = unit.GridPos;

        // Œ³‚Ìƒ}ƒX‚ğ‹ó‚É‚·‚é
        _cells[oldPos.x, oldPos.y].unit = null;

        // V‚µ‚¢ƒ}ƒX‚É’u‚­
        _cells[newPos.x, newPos.y].unit = unit;

        // Unit‘¤‚àXV
        unit.SetGridPos(newPos);
    }

    /// <summary>
    /// ˆÚ“®‚Å‚«‚é‚©‚Ç‚¤‚©‚ğbool‚Å”»’f‚·‚é
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

    //”Õ–ÊŠO‚©‚Ç‚¤‚©
    private bool IsInRange(Vector2Int pos)
    {
        return pos.x >= 0 && pos.x < width &&
               pos.y >= 0 && pos.y < height;
    }


>>>>>>> a074e9d56bed1d9b257ee15a8f711283f6fb7015
}
