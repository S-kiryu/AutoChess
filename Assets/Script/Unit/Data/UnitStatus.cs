using System;
using UnityEngine;

/// <summary>
/// ランタイム用のステータスクラス
/// </summary>
[Serializable]
public class UnitStatus
{
    [Header("基本ステータス")]
    private int _maxHp;
    private int _currentHp;
    private int _maxMp;
    private int _currentMp;
    private int _level;

    [Header("攻撃ステータス")]
    private float _attack;
    private float _magicAttack;
    private int _attackSpeed;
    private int _attackRange;

    [Header("クリティカル率")]
    private float _criticalRate;
    private float _criticalDamage;

    [Header("防御ステータス")]
    private float _defense;
    private float _magicDefense;
    private float _criticalDefense;

    [Header("サブステータス")]
    private float _moveSpeed;
    private float _dodgeRate;

    [Header("ユニットのタイプ")]
    private UnitType[] _type;

    public int MaxHp => _maxHp;
    public int CurrentHp => _currentHp;
    public int MaxMp => _maxMp;
    public int CurrentMp => _currentMp;
    public int Level => _level;
    public float Attack => _attack;
    public float MagicAttack => _magicAttack;
    public int AttackSpeed => _attackSpeed;
    public int AttackRange => _attackRange;
    public float CriticalRate => _criticalRate;
    public float CriticalDamage => _criticalDamage;
    public float Defense => _defense;
    public float MagicDefense => _magicDefense;
    public float CriticalDefense => _criticalDefense;
    public float MoveSpeed => _moveSpeed;
    public float DodgeRate => _dodgeRate;
    public UnitType[] Type => _type;

    public void Initialize(BaseStatus baseStatus)
    {

        _maxHp = baseStatus.Hp;
        _currentHp = baseStatus.Hp;
        _maxMp = baseStatus.Mp;
        _currentMp = 0;
        _level = baseStatus.Level;
        _attack = baseStatus.Attack;
        _magicAttack = baseStatus.MagicAttack;
        _attackSpeed = baseStatus.AttackSpeed;
        _attackRange = baseStatus.AttackRange;
        _criticalRate = baseStatus.CriticalRate;
        _criticalDamage = baseStatus.CriticalDamage;
        _defense = baseStatus.Defense;
        _magicDefense = baseStatus.MagicDefense;
        _criticalDefense = baseStatus.CriticalDefense;
        _moveSpeed = baseStatus.MoveSpeed;
        _dodgeRate = baseStatus.DodgeRate;
        _type = baseStatus.Type;
    }
}