using System.Collections.Generic;
using UnityEngine;

public class ShopManager : MonoBehaviour
{
    public List<UnitInstance> unitPool;

    private void Start()
    {
        unitPool = BattleUnitList.instance.GetUnits();

        if (unitPool == null)
        {
            Debug.LogError("unitPool が null です。BattleUnitList.instance.GetUnits() を確認してください");
            return;
        }

        Debug.Log($"unitPool.Count: {unitPool.Count}");

        for (int i = 0; i < unitPool.Count; i++)
        {
            Debug.Log($"{i}: {unitPool[i]?.Data?.CharacterName}");
        }
    }

    /// <summary>
    /// ユニットをランダムに生成する
    /// </summary>
    public UnitInstance GenerateShop()
    {
        if (unitPool == null)
        {
            Debug.LogWarning("unitPool が null なので生成できません");
            return null;
        }

        if (unitPool.Count == 0)
        {
            Debug.LogWarning("unitPool が空なので生成できません");
            return null;
        }

        int randomNum = Random.Range(0, unitPool.Count);

        UnitInstance data = unitPool[randomNum];

        if (data == null)
        {
            Debug.LogWarning($"unitPool[{randomNum}] が null です");
            return null;
        }

        if (data.Data == null)
        {
            Debug.LogWarning($"unitPool[{randomNum}] の CharacterData が null です");
            return null;
        }

        UnitInstance unit = UnitFactory.Create(data.Data);

        Debug.Log($"生成されたユニット: {unit.Data.CharacterName}");

        return unit;
    }
}