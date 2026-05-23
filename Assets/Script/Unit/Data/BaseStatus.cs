using UnityEngine;

[CreateAssetMenu(fileName = "BaseStatus", menuName = "ScriptableObjects/BaseStatus", order = 1)]
public class BaseStatus : ScriptableObject
{
    [Header("基本ステータス")]
    public int Hp;
    public int Mp;
    public int Level;

    [Header("攻撃ステータス")]
    public float Attack;
    public float MagicAttack;
    public int AttackSpeed;
    public int AttackRange;

    [Header("クリティカル率")]
    public float CriticalRate;
    public float CriticalDamage;

    [Header("防御ステータス")]
    public float Defense;
    public float MagicDefense;
    public float CriticalDefense;

    [Header("サブステータス")]
    public float MoveSpeed;
    /// 回避率
    public float dodgeRate;

    [Header("ユニットのタイプ")]
    public UnitType[] Type;
}


public enum UnitType
{
    Human,
    Elf,
    Orc,
    Undead,
    Beast
}
