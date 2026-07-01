using System.Collections.Generic;
using UnityEngine;

public class BattleManager : MonoBehaviour
{
    public static BattleManager Instance { get; private set; }

    // 戦闘にいるユニットを管理するリスト
    private List<BattleUnitBase> playerUnits = new List<BattleUnitBase>();
    private List<BattleUnitBase> enemyUnits = new List<BattleUnitBase>();

    private bool isBattleFinished;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    // 戦闘ユニットの追加
    public void RegisterUnit(BattleUnitBase unit)
    {
        if (unit == null)
        {
            return;
        }

        List<BattleUnitBase> targetList =
            unit.Team == BattleTeam.Player ? playerUnits : enemyUnits;

        if (targetList.Contains(unit))
        {
            return;
        }

        targetList.Add(unit);
    }

    // 戦闘ユニットの削除
    public void UnregisterUnit(BattleUnitBase unit)
    {
        if (unit == null)
        {
            return;
        }

        playerUnits.Remove(unit);
        enemyUnits.Remove(unit);
    }

    // 敵を探す
    public IReadOnlyList<BattleUnitBase> GetEnemies(BattleTeam team)
    {
        return team == BattleTeam.Player ? enemyUnits : playerUnits;
    }

    // 戦闘開始
    public void StartBattle()
    {
        isBattleFinished = false;

        if (BattleGridManager.Instance != null)
        {
            BattleGridManager.Instance.RegisterPlayerUnitsForBattle();
        }

        foreach (BattleUnitBase unit in playerUnits)
        {
            if (unit != null && !unit.IsDead)
            {
                unit.StartBattle();
            }
        }

        foreach (BattleUnitBase unit in enemyUnits)
        {
            if (unit != null && !unit.IsDead)
            {
                unit.StartBattle();
            }
        }
    }

    // ユニットが死んだときに呼ばれる
    public void NotifyUnitDead(BattleUnitBase deadUnit)
    {
        CheckBattleResult();
    }

    // 勝敗判定
    private void CheckBattleResult()
    {
        if (isBattleFinished)
        {
            return;
        }

        bool playerAllDead = IsAllDead(playerUnits);
        bool enemyAllDead = IsAllDead(enemyUnits);

        if (enemyAllDead)
        {
            isBattleFinished = true;
            StopAllUnits();

            Debug.Log("勝利");

            if (GameLoopManager.Instance != null)
            {
                GameLoopManager.Instance.ChangeState(GameState.Reward);
            }
        }
        else if (playerAllDead)
        {
            isBattleFinished = true;
            StopAllUnits();

            Debug.Log("敗北");
        }
    }

    private bool IsAllDead(List<BattleUnitBase> units)
    {
        if (units.Count == 0)
        {
            return false;
        }

        foreach (BattleUnitBase unit in units)
        {
            if (unit != null && !unit.IsDead)
            {
                return false;
            }
        }

        return true;
    }

    private void StopAllUnits()
    {
        foreach (BattleUnitBase unit in playerUnits)
        {
            if (unit != null)
            {
                unit.StopBattle();
            }
        }

        foreach (BattleUnitBase unit in enemyUnits)
        {
            if (unit != null)
            {
                unit.StopBattle();
            }
        }
    }
}