using System.Collections.Generic;
using UnityEngine;

public class BattleManager : MonoBehaviour
{
    public static BattleManager Instance { get; private set; }

    //戦闘にいるユニットを管理するリスト
    private List<BattleUnitBase> playerUnits = new List<BattleUnitBase>();
    private List<BattleUnitBase> enemyUnits = new List<BattleUnitBase>();

    private void Awake()
    {
        Instance = this;
    }

    //
    public void RegisterUnit(BattleUnitBase unit)
    {
        if (unit.Team == BattleTeam.Player)
        {
            playerUnits.Add(unit);
        }
        else
        {
            enemyUnits.Add(unit);
        }
    }

    public List<BattleUnitBase> GetEnemies(BattleTeam BattleTeam)
    {
        return BattleTeam == BattleTeam.Player ? enemyUnits : playerUnits;
    }
}