using System.Collections.Generic;
using UnityEngine;

/// <summary>ユニットの解放を管理するクラス<summary>
public class UnitUnlockChecker
{
    Dictionary<UnitData, bool> _unlockChecker = new();

    public UnitUnlockChecker(UnitData[] unitData)
    {
        foreach (var data in unitData)
        {
            if (data != null)
                _unlockChecker[data] = false;
        }
    }

    public void UnlockUnit(UnitData data)
    {
        if (_unlockChecker.ContainsKey(data))
            _unlockChecker[data] = true;
    }

    public bool IsUnitUnlocked(UnitData data)
    {
        return _unlockChecker.TryGetValue(data, out var unlocked) && unlocked;
    }
}
