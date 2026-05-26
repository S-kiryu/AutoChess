using System;
using UnityEngine;

/// <summary>
/// ランタイム用のステータスクラス
/// </summary>
[Serializable]
public class UnitStatus
{
    [Header("基本ステータス")]
    [SerializeField] private int _maxHp;
    [SerializeField] private int _currentHp;
    [SerializeField] private int _maxMp;
    [SerializeField] private int _currentMp;
    [SerializeField] private int _level;

    [Header("攻撃ステータス")]
    [SerializeField] private float attack;
    [SerializeField] private float magicAttack;
    [SerializeField] private int attackSpeed;
    [SerializeField] private int attackRange;

    [Header("クリティカル率")]
    [SerializeField] private float criticalRate;
    [SerializeField] private float criticalDamage;

    [Header("防御ステータス")]
    [SerializeField] private float defense;
    [SerializeField] private float magicDefense;
    [SerializeField] private float criticalDefense;

    [Header("サブステータス")]
    [SerializeField] private float moveSpeed;
    [SerializeField] private float dodgeRate;

    [Header("ユニットのタイプ")]
    [SerializeField] private UnitType[] type;

    public int MaxHp => _maxHp;
    public int CurrentHp => _currentHp;
    public int MaxMp => _maxMp;
    public int CurrentMp => _currentMp;
    public int Level => _level;
    public float Attack => attack;
    public float MagicAttack => magicAttack;
    public int AttackSpeed => attackSpeed;
    public int AttackRange => attackRange;
    public float CriticalRate => criticalRate;
    public float CriticalDamage => criticalDamage;
    public float Defense => defense;
    public float MagicDefense => magicDefense;
    public float CriticalDefense => criticalDefense;
    public float MoveSpeed => moveSpeed;
    public float DodgeRate => dodgeRate;
    public UnitType[] Type => type;

    public void Initialize(BaseStatus baseStatus)
    {

        _maxHp = baseStatus.Hp;
        _currentHp = baseStatus.Hp;
        _maxMp = baseStatus.Mp;
        _currentMp = 0;
        _level = baseStatus.Level;
        attack = baseStatus.Attack;
        magicAttack = baseStatus.MagicAttack;
        attackSpeed = baseStatus.AttackSpeed;
        attackRange = baseStatus.AttackRange;
        criticalRate = baseStatus.CriticalRate;
        criticalDamage = baseStatus.CriticalDamage;
        defense = baseStatus.Defense;
        magicDefense = baseStatus.MagicDefense;
        criticalDefense = baseStatus.CriticalDefense;
        moveSpeed = baseStatus.MoveSpeed;
        dodgeRate = baseStatus.DodgeRate;
        type = baseStatus.Type;
    }
}