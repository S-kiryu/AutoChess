using UnityEngine;
using System.Collections.Generic;


public class BattleUnitList : MonoBehaviour
{
    public static BattleUnitList instance;

    [SerializeField] private FormationManager formationManager;
    private List<UnitInstance> _unitList = new List<UnitInstance>();
    private int[] a = new int[0];

    void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }
        DontDestroyOnLoad(gameObject);
        instance = this;
    }

    public void BattleUnitSet() 
    {
        var units = formationManager.GetUnits();
        foreach (var unit in units)
        {
            _unitList.Add(unit);
        }
    }
}
