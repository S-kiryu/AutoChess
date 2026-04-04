using System.Threading.Tasks;
using UnityEngine;

public class UnitModel
{
    public int CurrentHp { get; private set; }
    public int CurentMp { get; private set; }
    public int Level { get; private set; }

    public float attackBuff = 1.0f;
    public float defenseBuff = 1.0f;
    public float magicAttackBuff = 1.0f;
    public float magicDefenseBuff = 1.0f;
    public float criticalRateBuff = 1.0f;
    public float criticalDamageBuff = 1.0f;

    private BaseStatus _baseStatus;

    public UnitModel(BaseStatus baseStatus)
    {
        _baseStatus = baseStatus;
        CurrentHp = baseStatus.Hp;
        CurentMp = baseStatus.Mp;
        Level = baseStatus.Level;
    }

    public float Attack => _baseStatus.Attack * attackBuff;
    public float Defense => _baseStatus.Defense * defenseBuff;
    public float MagicAttack => (_baseStatus.MagicAttack * magicAttackBuff);
    public float MagicDefense => _baseStatus.MagicDefense * magicDefenseBuff;
    public float CriticalRate => _baseStatus.CriticalRate + criticalRateBuff;
    public float CriticalDamage => _baseStatus.CriticalDamage + criticalDamageBuff;
    public void TakeDamage(int damage)
    {
        CurrentHp = Mathf.Max(0, CurrentHp - damage);
    }
    #region//ƒoƒtŒn
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
