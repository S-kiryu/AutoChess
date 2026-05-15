using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class FormationManager : MonoBehaviour
{
    [SerializeField] private FormationSlot[] _slots;
    [SerializeField] private SceneAsset _sceneAsset;

    public void OnClick()
    {
        var units = GetBattleUnits();
        BattleDataManager.Instance.PlayerUnits = units;
        Debug.Log($"{_sceneAsset.name}に移動");
        UnityEngine.SceneManagement.SceneManager.LoadScene(_sceneAsset.name);
    }

    // ドロップされたユニットを取得するためのメソッド
    public List<UnitData> GetBattleUnits()
    {
        List<UnitData> units = new List<UnitData>();

        foreach (var slot in _slots)
        {
            if (slot.CurrentUnitData != null)
            {
                units.Add(slot.CurrentUnitData);
                Debug.Log($"Slot has unit: {slot.CurrentUnitData}");
            }
        }

        return units;
    }
}
