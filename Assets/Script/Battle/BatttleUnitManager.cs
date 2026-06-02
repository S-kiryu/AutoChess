using UnityEngine;

public class BatttleUnitManager : MonoBehaviour
{
    BattleUnitList _unitsL;

    private void Awake()
    {
        _unitsL = FindFirstObjectByType<BattleUnitList>();
        _unitsL.GetUnits();
    }
}
