using System.Collections.Generic;
using UnityEngine;

public class ShopManager : MonoBehaviour
{
    public List<UnitInstance> unitPool;

    private void Start()
    {
        unitPool = BattleUnitList.instance.GetUnits();
    }

    /// <summary>
    /// ユニットをランダムに生成する
    /// </summary>
    /// <param name="count"></param>
    /// <returns></returns>
    public UnitInstance GenerateShop()
    {

        var data = unitPool[Random.Range(0, unitPool.Count)];
        var unit = UnitFactory.Create(data.Data);

        var result = unit;

        return result;
    }

}
