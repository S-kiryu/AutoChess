using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopFactory : MonoBehaviour
{
    [SerializeField] Button _button;
    [SerializeField] ShopManager _shopManager;
    [SerializeField] BenchManager _benchManager;

    public void OnClickGenerate()
    {
        UnitInstance unit = _shopManager.GenerateShop();

        if (unit == null)
        {
            Debug.LogWarning("ユニット生成に失敗しました");
            return;
        }

        bool placed = _benchManager.TryAddUnit(unit);

        if (!placed)
        {
            Debug.Log("ベンチが満杯なので配置できません");
            return;
        }

        Debug.Log($"ユニット {unit.Data.CharacterName} をベンチに配置しました");
    }
}
