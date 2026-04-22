using UnityEngine;

public class UnitModel
{
    public int CurrentHp { get; private set; }
    public int CurentMp { get; private set; }
    public int Level { get; private set; }
    public TeamType Team { get; private set; }
    public Vector2Int GridPos { get; private set; }

    public float attackBuff = 1.0f;
    public float defenseBuff = 1.0f;
    public float magicAttackBuff = 1.0f;
    public float magicDefenseBuff = 1.0f;
    public float criticalRateBuff = 0f;
    public float criticalDamageBuff = 0f;

    private BaseStatus _baseStatus;

    public UnitModel(BaseStatus baseStatus, TeamType team)
    {
        _baseStatus = baseStatus;
        Team = team;

        CurrentHp = baseStatus.Hp;
        CurentMp = baseStatus.Mp;
        Level = baseStatus.Level;
    }

    public void MoveTo(Vector2Int pos)
    {
        GridPos = pos;
    }

    public enum TeamType
    {
        Player,
        Enemy
    }

    public float Attack => _baseStatus.Attack * attackBuff;
    public int AttackSpeed => _baseStatus.AttackSpeed;
    public int AttackRange => _baseStatus.AttackRange;
    public float MoveSpeed => _baseStatus.MoveSpeed;
    public float Defense => _baseStatus.Defense * defenseBuff;
    public float MagicAttack => (_baseStatus.MagicAttack * magicAttackBuff);
    public float MagicDefense => _baseStatus.MagicDefense * magicDefenseBuff;
    public float CriticalRate => _baseStatus.CriticalRate + criticalRateBuff;
    public float CriticalDamage => _baseStatus.CriticalDamage + criticalDamageBuff;
    public void TakeDamage(float damage)
    {
        CurrentHp = Mathf.Max(0, Mathf.RoundToInt(CurrentHp - damage));
    }

    public void SetGridPos(Vector2Int pos)
    {
        GridPos = pos;
    }

    #region//バフ系
    public void AttackBuff(float buff)
    {
        attackBuff += buff;
    }

    public void DefenseBuff(float buff)
    {
        defenseBuff += buff;
    }

    public void MagicAttackBuff(float buff)
    {
        magicAttackBuff += buff;
    }

    public void MagicDefenseBuff(float buff)
    {
        magicDefenseBuff += buff;
    }
    public void CriticalRateBuff(float buff)
    {
        criticalRateBuff += buff;
    }
    public void CriticalDamageBuff(float buff)
    {
        criticalDamageBuff += buff;
    }
    #endregion
}

