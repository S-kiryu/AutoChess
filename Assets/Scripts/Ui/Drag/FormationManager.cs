using System.Collections.Generic;
using UnityEngine;

public class FormationManager : MonoBehaviour
{
    [SerializeField] private FormationSlot[] _slots;

    public List<UnitData> GetBattleUnits()
    {
        List<UnitData> units = new List<UnitData>();

        foreach (var slot in _slots)
        {
            if (slot.CurrentUnitData != null)
            {
                units.Add(slot.CurrentUnitData);
            }
        }

        return units;
    }
}
