using System.Collections.Generic;
using UnityEngine;

public class BattleManager : MonoBehaviour
{
    public static BattleManager Instance { get; private set; }

    [SerializeField] private BattleUnitSpawner battleUnitSpawner;
    [SerializeField] private BattleMovementResolver movementResolver;
    [SerializeField] private StageProgressManager stageProgressManager;

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
    public List<BattleUnitBase> GetAllBattleUnits()
    {
        List<BattleUnitBase> allUnits = new List<BattleUnitBase>();

        allUnits.AddRange(playerUnits);
        allUnits.AddRange(enemyUnits);

        return allUnits;
    }

    /// <summary>
    /// ユニットを追加する関数
    /// </summary>
    /// <param name="unit">追加するユニット</param>
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

    /// <summary>
    /// 戦闘ユニットを削除する関数
    /// </summary>
    /// <param name="unit">削除するユニット</param>
    public void UnregisterUnit(BattleUnitBase unit)
    {
        if (unit == null)
        {
            return;
        }

        playerUnits.Remove(unit);
        enemyUnits.Remove(unit);
    }

    /// <summary>
    /// 敵を探す関数
    /// </summary>
    /// <param name="team">探索するチーム</param>
    /// <returns>敵のリスト</returns>
    public IReadOnlyList<BattleUnitBase> GetEnemies(BattleTeam team)
    {
        return team == BattleTeam.Player ? enemyUnits : playerUnits;
    }

    /// <summary>
    /// 戦闘開始関数
    /// </summary>
    public void StartBattle()
    {
        Debug.Log("BattleManager.StartBattle");

        isBattleFinished = false;

        if (battleUnitSpawner != null)
        {
            battleUnitSpawner.RegisterPlayerUnits();
        }

        Debug.Log($"戦闘開始時 味方数: {playerUnits.Count}, 敵数: {enemyUnits.Count}");

        foreach (BattleUnitBase unit in playerUnits)
        {
            if (unit != null && !unit.IsDead)
            {
                unit.StartBattle();
            }
            else if (unit != null && unit.IsDead)
            {
                Debug.Log("味方ユニットがすでに死んでいます");
            }
        }

        foreach (BattleUnitBase unit in enemyUnits)
        {
            if (unit != null && !unit.IsDead)
            {
                unit.StartBattle();
            }
            else if (unit != null && unit.IsDead)
            {
                Debug.Log("敵ユニットがすでに死んでいます");
            }
        }
        if (movementResolver != null)
        {
            movementResolver.StartResolve();
        }

    }

    /// <summary>
    /// ユニットが死んだときに呼ばれる関数
    /// </summary>
    /// <param name="deadUnit">死亡したユニット</param>
    public void NotifyUnitDead(BattleUnitBase deadUnit)
    {
        CheckBattleResult();
    }

    /// <summary>
    /// 勝敗判定
    /// </summary>
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

            if (battleUnitSpawner != null)
            {
                battleUnitSpawner.RestorePlayerUnitsAfterBattle();
            }

            Debug.Log("勝利");

            if (GameLoopManager.Instance != null)
            {
                GameLoopManager.Instance.ChangeState(GameState.Reward);
            }

            if (stageProgressManager != null)
            {
                stageProgressManager.NextBattleStage();
            }
        }
        else if (playerAllDead)
        {
            isBattleFinished = true;
            StopAllUnits();

            if (GameLoopManager.Instance != null)
            {
                GameLoopManager.Instance.ChangeState(GameState.Reward);
            }

            if (battleUnitSpawner != null)
            {
                battleUnitSpawner.RestorePlayerUnitsAfterBattle();
                battleUnitSpawner.RestoreEnemyUnitsAfterBattle();
            }

            Debug.Log("敗北");
        }
    }
    
    /// <summary>
    /// すべてのユニットが死亡しているか判定する関数
    /// </summary>
    /// <param name="units">判定するユニットのリスト</param>
    /// <returns>すべて死亡している場合はtrue、そうでない場合はfalse</returns>
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

    /// <summary>
    /// すべてのユニットを停止する関数
    /// </summary>
    private void StopAllUnits()
    {
        if (movementResolver != null)
        {
            movementResolver.StopResolve();
        }

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