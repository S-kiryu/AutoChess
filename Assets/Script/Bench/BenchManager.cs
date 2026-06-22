using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ベンチのセットデータなどを管理するクラス
/// </summary>
public class BenchManager : MonoBehaviour
{
    private static BenchManager instance;

    //ベンチサイズ
    [SerializeField] private int width = 8;
    [SerializeField] private int height = 1;

    private UnitInstance[,] _benchs;
    private BattleGridManager battleGridManager;

    public int Width => width;
    public int Height => height;

    //置いた時
    public event System.Action<UnitInstance, int, int> OnUnitPlaced;
    //撤去したとき
    public event System.Action<UnitInstance, int, int> OnUnitRemoved;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            // ベンチの初期化
            _benchs = new UnitInstance[width, height];
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // ベンチに置いてあるユニットのリストを取得する
    public List<UnitInstance> GetUnits()
    {
        List<UnitInstance> units = new List<UnitInstance>();

        for (int y = 0; y < _benchs.GetLength(1); y++)
        {
            for (int x = 0; x < _benchs.GetLength(0); x++)
            {
                UnitInstance unit = _benchs[x, y];

                if (unit != null)
                {
                    units.Add(unit);
                }
            }
        }

        return units;
    }

    public bool SetUnit(UnitInstance unit, int x, int y)
    {
        if (!IsInside(x, y))
        {
            return false;
        }

        // すでにユニットが配置されている場合は置けない
        if (_benchs[x, y] != null)
        {
            return false;
        }

        _benchs[x, y] = unit;
        OnUnitPlaced?.Invoke(unit, x, y);
        return true;
    }

    public bool RemoveUnit(int x, int y)
    {
        if (!IsInside(x, y))
        {
            return false;
        }

        UnitInstance unit = _benchs[x, y];

        if (unit == null)
        {
            return false;
        }

        _benchs[x, y] = null;
        OnUnitRemoved?.Invoke(unit, x, y);
        return true;
    }

    //指定した位置にいるユニットを入れ替える
    public bool SwapUnit(BenchSlotUI draggedUI,int x,int y) 
    {
        draggedUI.SetDropped(true);

        int fromX = draggedUI.X;
        int fromY = draggedUI.Y;

        if (fromX == x && fromY == y) return false;

        var movingUnit = draggedUI.Unit;
        var targetUnit = GetUnit(x, y);

        RemoveUnit(fromX, fromY);

        if (targetUnit != null)
        {
            RemoveUnit(x, y);
            SetUnit(targetUnit, fromX, fromY);
        }

        SetUnit(movingUnit, x, y);

        return true;
    }

    /// <summary>
    /// 空いてるベンチを探してユニットを置く
    /// </summary>
    /// <param name="unit"></param>
    /// <returns></returns>
    public bool TryAddUnit(UnitInstance unit)
    {
        for (int y = 0; y < _benchs.GetLength(1); y++)
        {
            for (int x = 0; x < _benchs.GetLength(0); x++)
            {
                if (SetUnit(unit, x, y))
                {
                    return true;
                }
            }
        }

        return false;
    }

    /// 指定した位置のユニットを取得する
    public UnitInstance GetUnit(int x, int y)
    {
        if (!IsInside(x, y))
        {
            return null;
        }

        return _benchs[x, y];
    }

    public void MoveUnitFromGrid(int x, int y)
    {
        battleGridManager.RemoveUnit(x, y);
    }

    /// <summary>
    /// ベンチの範囲内かどうかをチェックする
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <returns></returns>
    private bool IsInside(int x, int y)
    {
        return x >= 0 && x < _benchs.GetLength(0)
            && y >= 0 && y < _benchs.GetLength(1);
    }
}