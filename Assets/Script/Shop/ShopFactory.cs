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
        var Unit = _shopManager.GenerateShop();
        bool placed = _benchManager.TryAddUnit(Unit);

        if (!placed)
        {
            Debug.Log("ベンチが満杯なので配置できません");
        }
        else
        {
            Debug.Log($"ユニット {Unit.Data.CharacterName} をベンチに配置しました");
        }
    }
}
