using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ベンチ上のユニットデータを管理する。
/// 通常移動では削除イベントを発生させない。
/// </summary>
public class BenchManager : MonoBehaviour
{
    public static BenchManager Instance { get; private set; }

    //ベンチのサイズ
    [SerializeField] private int width = 4;
    [SerializeField] private int height = 2;

    private UnitInstance[,] benchUnits;

    public int Width => width;
    public int Height => height;

    // 新しく購入したユニットなどを生成するとき
    public event System.Action<UnitInstance, int, int> OnUnitPlaced;

    // 売却して完全に削除するとき
    public event System.Action<UnitInstance, int, int> OnUnitRemoved;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        benchUnits = new UnitInstance[width, height];
    }

    public List<UnitInstance> GetUnits()
    {
        List<UnitInstance> units = new List<UnitInstance>();

        for (int y = 0; y < benchUnits.GetLength(1); y++)
        {
            for (int x = 0; x < benchUnits.GetLength(0); x++)
            {
                UnitInstance unit = benchUnits[x, y];

                if (unit != null)
                {
                    units.Add(unit);
                }
            }
        }

        return units;
    }

    public UnitInstance GetUnit(int x, int y)
    {
        if (!IsInside(x, y))
        {
            return null;
        }

        return benchUnits[x, y];
    }

    /// <summary>
    /// 新しいユニットをベンチに追加する。
    /// UI生成イベントも発生する。
    /// </summary>
    public bool SetUnit(UnitInstance unit, int x, int y)
    {
        if (!PutUnit(unit, x, y))
        {
            return false;
        }

        OnUnitPlaced?.Invoke(unit, x, y);
        return true;
    }

    /// <summary>
    /// データだけをベンチへ配置する。
    /// 移動時はこちらを使用する。
    /// </summary>
    public bool PutUnit(UnitInstance unit, int x, int y)
    {
        if (unit == null || !IsInside(x, y))
        {
            return false;
        }

        if (benchUnits[x, y] != null)
        {
            return false;
        }

        benchUnits[x, y] = unit;
        return true;
    }

    /// <summary>
    /// データだけをスロットから取り出す。
    /// UIは削除しない。
    /// </summary>
    public UnitInstance TakeUnit(int x, int y)
    {
        if (!IsInside(x, y))
        {
            return null;
        }

        UnitInstance unit = benchUnits[x, y];

        if (unit == null)
        {
            return null;
        }

        benchUnits[x, y] = null;
        return unit;
    }

    /// <summary>
    /// ベンチ内でデータを交換する。
    /// </summary>
    public bool SwapUnits(
        int fromX,
        int fromY,
        int toX,
        int toY)
    {
        if (!IsInside(fromX, fromY) ||
            !IsInside(toX, toY))
        {
            return false;
        }

        if (fromX == toX && fromY == toY)
        {
            return false;
        }

        UnitInstance movingUnit = benchUnits[fromX, fromY];

        if (movingUnit == null)
        {
            return false;
        }

        UnitInstance targetUnit = benchUnits[toX, toY];

        benchUnits[toX, toY] = movingUnit;
        benchUnits[fromX, fromY] = targetUnit;

        return true;
    }

    /// <summary>
    /// ユニットを売却して完全に削除する。
    /// Destroyにつながるイベントはここだけで発生させる。
    /// </summary>
    public bool SellUnit(int x, int y)
    {
        UnitInstance unit = TakeUnit(x, y);

        if (unit == null)
        {
            return false;
        }

        // 必要ならここで所持金を追加する
        // GoldManager.Instance.AddGold(unit.Data.SellPrice);

        OnUnitRemoved?.Invoke(unit, x, y);
        return true;
    }

    /// <summary>
    /// 
    /// </summary>
    public bool TryAddUnit(UnitInstance unit)
    {
        for (int y = 0; y < benchUnits.GetLength(1); y++)
        {
            for (int x = 0; x < benchUnits.GetLength(0); x++)
            {
                if (SetUnit(unit, x, y))
                {
                    return true;
                }
            }
        }

        return false;
    }

    /// <summary>
    /// グリットの範囲内かどうかを判定する。
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <returns></returns>
    public bool IsInside(int x, int y)
    {
        return x >= 0 &&
               x < benchUnits.GetLength(0) &&
               y >= 0 &&
               y < benchUnits.GetLength(1);
    }
}