using System.Collections.Generic;
using UnityEditor.Overlays;
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
        List<BattleUnitBase> enemies = new List<BattleUnitBase>();

        //適した位置に敵を生成している
        foreach (EnemySpawnData spawnData in stageData.Enemies)
        {
            Vector3 spawnPosition = BattleGridManager.Instance.GetEnemyWorldPosition(
                spawnData.GridPosition.x,
                spawnData.GridPosition.y);

            BattleUnitBase enemy = Instantiate(
                enemyPrefab,
                spawnPosition,
                Quaternion.identity,
                enemyParent
            );

            UnitInstance enemyInstance = new UnitInstance();
            enemyInstance.Initialize(spawnData.CharacterData);
            Debug.Log($"{enemyInstance.Data.name}を{spawnData.GridPosition.x}{spawnData.GridPosition.y}に生成");

            enemy.Initialize(enemyInstance, teamId: BattleTeam.Enemy);

            enemies.Add(enemy);
        }

        return enemies;
    }
}