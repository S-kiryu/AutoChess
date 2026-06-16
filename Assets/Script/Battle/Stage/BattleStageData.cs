using System;
using UnityEngine;

/// <summary>
/// ステージを保存するオブジェクト
/// </summary>
[CreateAssetMenu(menuName = "AutoChess/BattleStageData")]
public class BattleStageData : ScriptableObject
{
    [SerializeField] private string stageId;
    [SerializeField] private EnemySpawnData[] enemies;
    [SerializeField] private bool isBossStage;

    public string StageId => stageId;
    public EnemySpawnData[] Enemies => enemies;
    public bool IsBossStage => isBossStage;
}