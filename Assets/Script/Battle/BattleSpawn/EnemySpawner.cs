using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 戦闘時に敵を生成するクラス
/// </summary>
public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private BattleUnitBase enemyPrefab;
    [SerializeField] private Transform enemyParent;

    /// <summary>
    /// ステージのデータを読み込み適したところにユニットを生成するクラス
    /// </summary>
    /// <param name="stageData">生成したいステージ</param>
    /// <returns></returns>
    public List<BattleUnitBase> SpawnEnemies(BattleStageData stageData)
    {
        Debug.Log($"ステージ{stageData.StageId}の敵を生成");
        List<BattleUnitBase> enemies = new List<BattleUnitBase>();

        //適した位置に敵を生成している
        foreach (EnemySpawnData spawnData in stageData.Enemies)
        {
            BattleGrid targetGrid = BattleGridManager.Instance.GetEnemyGrid(
                spawnData.GridPosition.x,
                spawnData.GridPosition.y);

            if (targetGrid == null)
            {
                Debug.LogWarning(
                    $"敵の生成位置が範囲外です: {spawnData.GridPosition.x}, {spawnData.GridPosition.y}");
                continue;
            }

            BattleUnitBase enemy = Instantiate(
                enemyPrefab,
                BattleGridManager.Instance.BattleUnitRoot);

            enemy.transform.position = targetGrid.transform.position;
            enemy.transform.localRotation = Quaternion.identity;
            enemy.transform.localScale = Vector3.one;
            enemy.SetCurrentGrid(targetGrid);

            UnitInstance enemyInstance = new UnitInstance();
            enemyInstance.Initialize(spawnData.CharacterData);
            Debug.Log($"{enemyInstance.Data.name}を{spawnData.GridPosition.x}{spawnData.GridPosition.y}に生成");

            enemy.Initialize(enemyInstance, teamId: BattleTeam.Enemy);

            enemies.Add(enemy);
        }

        return enemies;
    }
}