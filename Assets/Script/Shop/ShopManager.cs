using System.Collections.Generic;
using UnityEngine;

public class ShopManager : MonoBehaviour
{
    public List<UnitInstance> unitPool;
    [SerializeField] private int _generate = 1;

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
            Debug.Log($"Generated Unit: {instance.Data.name}");
        }
    }
}
