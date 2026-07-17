using UnityEngine;

[System.Serializable]
public class StarGradeData
{
    [Min(1)] public int Star = 1;
    int RequiredUnitCount = 2;

    [Header("倍率")]
    [Min(1f)] public float HpRate = 1f;
    [Min(1f)] public float AttackRate = 1f;
    [Min(1f)] public float MagicAttackRate = 1f;
    [Min(1f)] public float DefenseRate = 1f;
    [Min(1f)] public float MagicDefenseRate = 1f;
}