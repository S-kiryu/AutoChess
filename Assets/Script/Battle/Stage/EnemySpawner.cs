using System.Collections.Generic;
using UnityEditor.Overlays;
using UnityEngine;

/// <summary>
/// 戦闘時に敵を生成するクラス
/// </summary>
public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private BattleUnitBase enemyPrefab;
    //[SerializeField] private BattleGrid battleGrid;
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
            //Vector3 spawnPosition = battleGrid.GetWorldPosition(spawnData.GridPosition);
            Vector3 spawnPosition = new Vector3();

            BattleUnitBase enemy = Instantiate(
                enemyPrefab,
                spawnPosition,
                Quaternion.identity,
                enemyParent
            );

            UnitInstance enemyInstance = new UnitInstance();
            enemyInstance.Initialize(spawnData.CharacterData);

            enemy.Initialize(enemyInstance, teamId: 1);

            enemies.Add(enemy);
        }

        return enemies;
    }
}