using System.Collections.Generic;
using UnityEngine;

/// <summary>ユニットの解放を管理するクラス<summary>
public class UnitUnlockChecker
{
    public UnitUnlockChecker Instance { get; private set; }

    Dictionary<UnitData, bool> UnlockChecker = new Dictionary<UnitData, bool>();

    //ユニットが解放済みかどうかを判定
    public UnitUnlockChecker(UnitData[] unitData)
    {
        for (int i = 0; i < unitData.GetLength(0); i++)
        {

            if (unitData[i] != null)
            {
                UnlockChecker[unitData[i]] = false;
            }

        }
    }

    //ユニットを解放する
    public void UnlockUnit(UnitData data)
    {
        if (UnlockChecker.ContainsKey(data))
        {
            UnlockChecker[data] = true;
        }
    }

    //ユニットが解放されているかどうかを返す,解放されていない場合はfalseを返す
    public bool IsUnitUnlocked(UnitData data)
    {
        if (UnlockChecker.ContainsKey(data))
        {
            return UnlockChecker[data];
        }
        return false;
    }
}
