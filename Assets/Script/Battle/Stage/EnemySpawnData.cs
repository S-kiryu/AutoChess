using System;
using UnityEngine;

/// <summary>
/// スポーンする敵と位置
/// </summary>
[Serializable]
public class EnemySpawnData
{
    [SerializeField] private CharacterData characterData;
    [SerializeField] private Vector2Int gridPosition;

    public CharacterData CharacterData => characterData;
    public Vector2Int GridPosition => gridPosition;
}