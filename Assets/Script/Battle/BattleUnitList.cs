using UnityEngine;
using System.Collections.Generic;


public class BattleUnitList : MonoBehaviour
{
    public static BattleUnitList instance { get; private set; }

    [SerializeField] private FormationManager formationManager;
    private List<UnitInstance> _unitList = new List<UnitInstance>();

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

    //ƒ{ƒ^ƒ“‚ÅŒÄ‚ñ‚Å‚Ü‚·
    public void BattleUnitSet() 
    {
        var units = formationManager.GetUnits();
        foreach (var unit in units)
        {
            _unitList.Add(unit);
        }
    }

    public void GetUnits() 
    {
        if(_unitList.Count < 0&&_unitList == null)return;
        foreach (var unit in _unitList) 
        {
            Debug.Log(unit);
        }
    }
}
