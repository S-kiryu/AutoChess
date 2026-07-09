using UnityEngine;

[System.Serializable]
public class LevelUpStatusData
{
    [Header("加算ステータス")]
    public int AddHp;
    public int AddMp;

    public float AddAttack;
    public float AddMagicAttack;
    public float AddDefense;
    public float AddMagicDefense;

    public float AddCriticalRate;
    public float AddCriticalDamage;
    public float AddCriticalDefense;

    public float AddMoveSpeed;
    public float AddDodgeRate;
}