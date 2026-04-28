using System.Collections.Generic;
using UnityEngine;

/// <summary>ユニットの解放を管理するクラス<summary>
public class UnitUnlockChecker
{
    public UnitUnlockChecker Instance { get; private set; }

    Dictionary<UnitData, bool> UnlockChecker = new Dictionary<UnitData, bool>();
    public UnitUnlockChecker(UnitData[] unitData)
    {
        foreach (var data in unitData)
        {
            UnlockChecker.Add(data, false);
        }
    }

    public void UnlockUnit(UnitData data)
    {
        if (UnlockChecker.ContainsKey(data))
        {
            UnlockChecker[data] = true;
        }
    }

    public bool IsUnitUnlocked(UnitData data)
    {
        if (UnlockChecker.ContainsKey(data))
        {
            return UnlockChecker[data];
        }
        return false;
    }
}
