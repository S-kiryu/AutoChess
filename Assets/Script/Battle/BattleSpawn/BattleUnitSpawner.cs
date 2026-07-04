using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 戦闘時に使うユニットを生成するクラス
/// </summary>
public class BattleUnitSpawner : MonoBehaviour
{
    //リセットデータ型
    private class PlayerUnitRestoreData
    {
        public BattleUnitBase BattleUnit;
        public BenchSlotUI UnitUI;
        public Transform Parent;
        public Vector2 AnchoredPosition;
        public BattleGrid Grid;
    }
    private class EnemyUnitRestoreData
    {
        public BattleUnitBase BattleUnit;
        public BattleGrid Grid;
    }

    private readonly List<PlayerUnitRestoreData> playerRestoreData =
        new List<PlayerUnitRestoreData>();

    private readonly List<EnemyUnitRestoreData> enemyRestoreData =
        new List<EnemyUnitRestoreData>();

    [SerializeField] private BattleUnitBase enemyPrefab;
    [SerializeField] private Transform battleUnitRoot;

    //配置した位置を覚えておくリスト
    private List<BattleGrid> playerUnitGrids = new List<BattleGrid>();
    private List<BattleGrid> enemyUnitGrids = new List<BattleGrid>();

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

            enemyRestoreData.Add(new EnemyUnitRestoreData
            {
                BattleUnit = enemy,
                Grid = targetGrid
            });

            enemies.Add(enemy);
            enemyUnitGrids.Add(targetGrid);
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

            RectTransform rectTransform =
                unitUI.GetComponent<RectTransform>();

            playerRestoreData.Add(new PlayerUnitRestoreData
            {
                BattleUnit = battleUnit,
                UnitUI = unitUI,
                Parent = unitUI.transform.parent,
                AnchoredPosition = rectTransform != null
                    ? rectTransform.anchoredPosition
                    : Vector2.zero,
                Grid = grid
            });

            battleUnit.transform.SetParent(battleUnitRoot, true);

            SetupBattleUnit(
                battleUnit,
                unitUI.Unit.Data,
                grid,
                BattleTeam.Player,
                unitUI.Unit);

            playerUnits.Add(battleUnit);
            playerUnitGrids.Add(grid);
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

    /// <summary>
    /// ユニットをバトル後にリセットする
    /// </summary>
    public void RestorePlayerUnitsAfterBattle()
    {
        foreach (PlayerUnitRestoreData data in playerRestoreData)
        {
            if (data == null ||
                data.BattleUnit == null ||
                data.UnitUI == null)
            {
                continue;
            }

            data.BattleUnit.ResetAfterBattle(data.Grid);

            data.UnitUI.transform.SetParent(data.Parent, false);

            RectTransform rectTransform =
                data.UnitUI.GetComponent<RectTransform>();

            if (rectTransform != null)
            {
                rectTransform.anchoredPosition = data.AnchoredPosition;
                rectTransform.localScale = Vector3.one;
            }

            data.UnitUI.transform.SetAsLastSibling();
        }

        playerRestoreData.Clear();
    }

    /// <summary>
    /// 敵ユニットをバトル後にリセットする
    /// </summary>
    public void RestoreEnemyUnitsAfterBattle()
    {
        foreach (EnemyUnitRestoreData data in enemyRestoreData)
        {
            if (data == null || data.BattleUnit == null)
            {
                continue;
            }

            data.BattleUnit.ResetAfterBattle(data.Grid);
        }
    }
}