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
    private float _attackSpeed;
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
    public float AttackSpeed => _attackSpeed;
    public int AttackRange => _attackRange;
    public float CriticalRate => _criticalRate;
    public float CriticalDamage => _criticalDamage;
    public float Defense => _defense;
    public float MagicDefense => _magicDefense;
    public float CriticalDefense => _criticalDefense;
    public float MoveSpeed => _moveSpeed;
    public float DodgeRate => _dodgeRate;
    public UnitType[] Type => _type;


    public bool IsDead => _currentHp <= 0;
    public bool IsManaFull => _maxMp > 0 && _currentMp >= _maxMp;

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

    public void HPReset()
    {
        _currentHp = _maxHp;
    }

    public void MPReset()
    {
        _currentMp = 0;
    }

    public void SetLevel(int level)
    {
        _level = Mathf.Max(1, level);
    }

    public void RestoreCurrentHpRate(float hpRate)
    {
        _currentHp = Mathf.Clamp(
            Mathf.RoundToInt(_maxHp * Mathf.Clamp01(hpRate)),
            0,
            _maxHp
        );
    }

    public void SetCurrentMp(int currentMp)
    {
        _currentMp = Mathf.Clamp(currentMp, 0, _maxMp);
    }

    public void AddMana(int amount)
    {
        if (amount <= 0 || _maxMp <= 0 || IsDead)
        {
            return;
        }

        _currentMp += amount;

        if (_currentMp > _maxMp)
        {
            _currentMp = _maxMp;
        }
    }

    public void ConsumeAllMana()
    {
        _currentMp = 0;
    }

    public void Heal(float healAmount)
    {
        _currentHp += (int)healAmount;
        if (_currentHp > _maxHp)
        {
            _currentHp = _maxHp;
        }
    }

    public void TakeDamage(float damage)
    {
        _currentHp -= (int)damage;

        if (_currentHp < 0)
        {
            _currentHp = 0;
        }
    }

    /// <summary>
    /// ユニットの星の数に応じてステータスを変化させる
    /// </summary>
    /// <param name="star"></param>
    public void ApplyStar(StarGradeData grade)
    {
        if (grade == null)
        {
            return;
        }

        float hpRate = Mathf.Max(grade.HpRate, 1f);
        float attackRate = Mathf.Max(grade.AttackRate, 1f);
        float magicAttackRate = Mathf.Max(grade.MagicAttackRate, 1f);
        float defenseRate = Mathf.Max(grade.DefenseRate, 1f);
        float magicDefenseRate = Mathf.Max(grade.MagicDefenseRate, 1f);

        _maxHp = Mathf.RoundToInt(_maxHp * hpRate);
        _currentHp = _maxHp;

        _attack *= attackRate;
        _magicAttack *= magicAttackRate;
        _defense *= defenseRate;
        _magicDefense *= magicDefenseRate;
    }

    public void ApplyLevelUp(LevelUpStatusData levelData, int level)
    {
        if (levelData == null)
        {
            return;
        }

        _level = level;

        _maxHp += levelData.AddHp;
        _currentHp = _maxHp;

        _maxMp += levelData.AddMp;

        _attack += levelData.AddAttack;
        _magicAttack += levelData.AddMagicAttack;
        _defense += levelData.AddDefense;
        _magicDefense += levelData.AddMagicDefense;

        _criticalRate += levelData.AddCriticalRate;
        _criticalDamage += levelData.AddCriticalDamage;
        _criticalDefense += levelData.AddCriticalDefense;

        _moveSpeed += levelData.AddMoveSpeed;
        _dodgeRate += levelData.AddDodgeRate;
    }
}