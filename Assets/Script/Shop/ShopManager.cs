using System.Collections.Generic;
using UnityEngine;

public class ShopManager : MonoBehaviour
{
    public List<UnitInstance> unitPool;

    private void Start()
    {
        unitPool = BattleUnitList.instance.GetUnits();
        for (int i = 0; i < unitPool.Count; i++)
            Debug.Log($"{unitPool[i]}");
    }

    /// <summary>
    /// ユニットをランダムに生成する
    /// </summary>
    /// <param name="count"></param>
    /// <returns></returns>
    public UnitInstance GenerateShop()
    {

        int randomNum = Random.Range(0, unitPool.Count);
        UnitInstance data;
        UnitInstance unit;

        if (unitPool[randomNum] == null)
        {
            unit = null;
        }
        else
        {
             data = unitPool[Random.Range(0, unitPool.Count)];
             unit = UnitFactory.Create(data.Data);

            Debug.Log($"生成されたユニット: {unit.Data.CharacterName}");
        }

        return unit;
    }

}
