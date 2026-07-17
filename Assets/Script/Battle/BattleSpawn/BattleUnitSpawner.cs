using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 謌ｦ髣俶凾縺ｫ菴ｿ縺・Θ繝九ャ繝医ｒ逕滓・縺吶ｋ繧ｯ繝ｩ繧ｹ
/// </summary>
public class BattleUnitSpawner : MonoBehaviour
{
    //繝ｪ繧ｻ繝・ヨ繝・・繧ｿ蝙・
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
    [SerializeField] private DamagePopupManager damagePopupManager;

    //驟咲ｽｮ縺励◆菴咲ｽｮ繧定ｦ壹∴縺ｦ縺翫￥繝ｪ繧ｹ繝・
    private List<BattleGrid> playerUnitGrids = new List<BattleGrid>();
    private List<BattleGrid> enemyUnitGrids = new List<BattleGrid>();

    /// <summary>
    /// 謨ｵ繝ｦ繝九ャ繝医ｒ逕滓・縺吶ｋ
    /// </summary>
    /// <param name="stageData">謨ｵ繝ｦ繝九ャ繝医・繝・・繧ｿ</param>
    /// <returns></returns>
    public List<BattleUnitBase> SpawnEnemies(BattleStageData stageData)
    {
        List<BattleUnitBase> enemies = new List<BattleUnitBase>();

        if (stageData == null)
        {
            Debug.LogWarning("BattleStageData 縺後≠繧翫∪縺帙ｓ縲・");
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
                    $"謨ｵ縺ｮ逕滓・菴咲ｽｮ縺檎ｯ・峇螟悶〒縺・ {spawnData.GridPosition.x}, {spawnData.GridPosition.y}");
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

    public void ClearEnemyUnits()
    {
        foreach (EnemyUnitRestoreData data in enemyRestoreData)
        {
            if (data == null || data.BattleUnit == null)
            {
                continue;
            }

            if (data.BattleUnit.CurrentGrid != null)
            {
                data.BattleUnit.CurrentGrid.ClearBattleUnit(data.BattleUnit);
            }

            Destroy(data.BattleUnit.gameObject);
        }

        enemyRestoreData.Clear();
        enemyUnitGrids.Clear();
    }

    /// <summary>
    /// 繝励Ξ繧､繝､繝ｼ縺ｮ繝ｦ繝九ャ繝医ｒ逋ｻ骭ｲ縺吶ｋ
    /// </summary>
    /// <returns></returns>
    public List<BattleUnitBase> RegisterPlayerUnits()
    {
        List<BattleUnitBase> playerUnits = new List<BattleUnitBase>();

        BattleGridManager gridManager = BattleGridManager.Instance;

        if (gridManager == null)
        {
            Debug.LogWarning("BattleGridManager がありません");
            return playerUnits;
        }

        playerRestoreData.Clear();
        playerUnitGrids.Clear();

        foreach (BattleGrid grid in gridManager.GetPlayerBattleGrids())
        {
            BenchSlotUI unitUI = grid.GetComponentInChildren<BenchSlotUI>(true);

            if (unitUI == null || unitUI.Unit == null)
            {
                continue;
            }

            BattleUnitBase battleUnit = unitUI.GetComponent<BattleUnitBase>();

            if (battleUnit == null)
            {
                Debug.LogWarning("BenchSlotUI に BattleUnitBase がありません", unitUI);
                continue;
            }

            RectTransform rectTransform = unitUI.GetComponent<RectTransform>();

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

            unitUI.Unit.RecalculateStatus();
            battleUnit.transform.SetParent(battleUnitRoot, true);

            SetupBattleUnit(
                battleUnit,
                unitUI.Unit.Data,
                grid,
                BattleTeam.Player,
                unitUI.Unit);

            playerUnits.Add(battleUnit);
            playerUnitGrids.Add(grid);

            Debug.Log(
                $"{unitUI.Unit.Data.CharacterName} Lv:{unitUI.Unit.Level} Star:{unitUI.Unit.Star} " +
                $"HP:{unitUI.Unit.Status.MaxHp} ATK:{unitUI.Unit.Status.Attack}");
        }

        return playerUnits;
    }

    /// <summary>
    /// 繝舌ヨ繝ｫ譎ゅ↓繝ｦ繝九ャ繝医ｒ繧ｻ繝・ヨ繧｢繝・・縺吶ｋ
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

        unitInstance.RecalculateStatus();

        unit.transform.position = grid.transform.position;
        unit.transform.localRotation = Quaternion.identity;
        unit.transform.localScale = Vector3.one;

        unit.SetCurrentGrid(grid);
        unit.Initialize(unitInstance, team, damagePopupManager);
    }

    /// <summary>
    /// 繝ｦ繝九ャ繝医ｒ繝舌ヨ繝ｫ蠕後↓繝ｪ繧ｻ繝・ヨ縺吶ｋ
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
        playerUnitGrids.Clear();
    }

    /// <summary>
    /// 謨ｵ繝ｦ繝九ャ繝医ｒ繝舌ヨ繝ｫ蠕後↓繝ｪ繧ｻ繝・ヨ縺吶ｋ
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