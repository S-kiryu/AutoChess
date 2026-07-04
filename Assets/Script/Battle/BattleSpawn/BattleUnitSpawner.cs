using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 戦闘時に使うユニットを生成するクラス
/// </summary>
public class BattleUnitSpawner : MonoBehaviour
{
    [SerializeField] private BattleUnitBase enemyPrefab;
    [SerializeField] private Transform battleUnitRoot;

    /// <summary>
    /// 敵ユニットを生成する
    /// </summary>
    /// <param name="stageData">敵ユニットのデータ</param>
    /// <returns></returns>
    public List<BattleUnitBase> SpawnEnemies(BattleStageData stageData)
    {
        List<BattleUnitBase> enemies = new List<BattleUnitBase>();

        if (stageData == null)
        {
            Debug.LogWarning("BattleStageData がありません。");
            return enemies;
        }

        foreach (EnemySpawnData spawnData in stageData.Enemies)
        {
            BattleGrid targetGrid =
                BattleGridManager.Instance.GetEnemyGrid(
                    spawnData.GridPosition.x,
                    spawnData.GridPosition.y);

            if (targetGrid == null)
            {
                Debug.LogWarning(
                    $"敵の生成位置が範囲外です: {spawnData.GridPosition.x}, {spawnData.GridPosition.y}");
                continue;
            }

            BattleUnitBase enemy =
                Instantiate(enemyPrefab, battleUnitRoot);

            SetupBattleUnit(
                enemy,
                spawnData.CharacterData,
                targetGrid,
                BattleTeam.Enemy);

            enemies.Add(enemy);
        }

        return enemies;
    }

    /// <summary>
    /// プレイヤーのユニットを登録する
    /// </summary>
    /// <returns></returns>
    public List<BattleUnitBase> RegisterPlayerUnits()
    {
        List<BattleUnitBase> playerUnits =
            new List<BattleUnitBase>();

        BattleGridManager gridManager =
            BattleGridManager.Instance;

        if (gridManager == null)
        {
            Debug.LogWarning("BattleGridManager がありません。");
            return playerUnits;
        }

        foreach (BattleGrid grid in gridManager.GetPlayerBattleGrids())
        {
            BenchSlotUI unitUI =
                grid.GetComponentInChildren<BenchSlotUI>(true);

            if (unitUI == null || unitUI.Unit == null)
            {
                continue;
            }

            BattleUnitBase battleUnit =
                unitUI.GetComponent<BattleUnitBase>();

            if (battleUnit == null)
            {
                Debug.LogWarning("BenchSlotUI に BattleUnitBase がありません。", unitUI);
                continue;
            }

            battleUnit.transform.SetParent(battleUnitRoot, true);

            SetupBattleUnit(
                battleUnit,
                unitUI.Unit.Data,
                grid,
                BattleTeam.Player,
                unitUI.Unit);

            playerUnits.Add(battleUnit);
        }

        return playerUnits;
    }


    /// <summary>
    /// バトル時にユニットをセットアップする
    /// </summary>
    private void SetupBattleUnit(
        BattleUnitBase unit,
        CharacterData characterData,
        BattleGrid grid,
        BattleTeam team,
        UnitInstance existingInstance = null)
    {
        if (unit == null || characterData == null || grid == null)
        {
            return;
        }

        UnitInstance unitInstance = existingInstance;

        if (unitInstance == null)
        {
            unitInstance = new UnitInstance();
            unitInstance.Initialize(characterData);
        }

        unit.transform.position = grid.transform.position;
        unit.transform.localRotation = Quaternion.identity;
        unit.transform.localScale = Vector3.one;

        unit.SetCurrentGrid(grid);
        unit.Initialize(unitInstance, team);
    }
}