using System.Collections.Generic;
using UnityEngine;

public class ShopManager : MonoBehaviour
{
    private List<UnitInstance> unitPool;

    private void Start()
    {
        unitPool = BattleUnitList.instance.GetShopCandidateUnits();
    }

    public UnitInstance GenerateShop()
    {
        if (unitPool == null || unitPool.Count == 0)
        {
            Debug.LogWarning("unitPool が空なので生成できません");
            return null;
        }

        UnitInstance template = unitPool[Random.Range(0, unitPool.Count)];

        if (template == null || template.Data == null)
        {
            return null;
        }

        UnitInstance unit = UnitFactory.Create(template.Data);

        // Homeで強化されているレベルを反映
        unit.SetLevel(template.Level);

        Debug.Log(
            $"生成されたユニット: {unit.Data.CharacterName} Lv:{unit.Level}");

        return unit;
    }
}