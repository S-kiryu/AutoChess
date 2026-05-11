using System.Collections.Generic;
using UnityEngine;

public class FormationManager : MonoBehaviour
{
    [SerializeField] private FormationSlot[] _slots;
    private List<UnitData> _OrganizationUnits;

    public void OnCrick() 
    {
        GetBattleUnits();
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
