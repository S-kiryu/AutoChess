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

    public List<UnitInstance> GetBattleUnits()
    {
        return _unitList;
    }

    public List<CharacterData> GetShopCandidateCharacters()
    {
        List<CharacterData> characters = new List<CharacterData>();

        foreach (UnitInstance unit in _unitList)
        {
            if (unit == null || unit.Data == null)
            {
                continue;
            }

            if (!characters.Contains(unit.Data))
            {
                characters.Add(unit.Data);
            }
        }

        return characters;
    }

    public List<UnitInstance> GetShopCandidateUnits()
    {
        List<UnitInstance> units = new List<UnitInstance>();

        foreach (UnitInstance unit in _unitList)
        {
            if (unit == null || unit.Data == null)
            {
                continue;
            }

            units.Add(unit);
        }

        return units;
    }

    //ƒ{ƒ^ƒ“‚ÅŒÄ‚ñ‚Å‚Ü‚·
    public void BattleUnitSet()
    {
        _unitList.Clear();

        var units = formationManager.GetUnits();

        foreach (var unit in units)
        {
            if (unit == null)
            {
                continue;
            }

            unit.RecalculateStatus();
            _unitList.Add(unit);
        }
    }

    public List<UnitInstance> GetUnits() 
    {
        if(_unitList.Count < 0)return null;
        foreach (var unit in _unitList) 
        {
            Debug.Log(unit);
        }
        return _unitList;
    }
}
