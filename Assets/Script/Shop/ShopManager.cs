using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ShopManager : MonoBehaviour
{
    public List<UnitInstance> unitPool;

    private void Start()
    {
        unitPool = BattleUnitList.instance.GetUnits();
    }

    //ユニットを生成するメソッド
    public void GenerateUnit(int count)
    {

        for (int i = 0; i < count; i++)
        {
            var baseUnit = unitPool[Random.Range(0, unitPool.Count)];

            var instance = new UnitInstance();
            instance.Initialize(baseUnit.Data);
        }
    }
}
