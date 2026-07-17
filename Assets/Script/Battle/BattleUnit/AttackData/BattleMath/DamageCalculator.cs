using UnityEngine;

public static class DamageCalculator
{
    /// <summary>
    /// 攻撃の計算をする関数
    /// </summary>
    /// <param name="attacker">攻撃する側</param>
    /// <param name="defender">受ける側</param>
    /// <param name="damageType"></param>
    /// <returns></returns>
    public static DamageResult CalculateDamage(
     BattleUnitBase attacker,
     BattleUnitBase defender,
     DamageType damageType,
     float damageMultiplier = 1f)
    {
        if (attacker == null ||
            defender == null ||
            attacker.Status == null ||
            defender.Status == null)
        {
            return DamageResult.Zero;
        }

        if (Random.value < defender.Status.DodgeRate)
        {
            return new DamageResult(0, false, true);
        }

        //タイプに適しているステータスを使ってダメージ計算
        float damage = damageType switch
        {
            DamageType.Physical => attacker.Status.Attack - defender.Status.Defense,
            DamageType.Magic => attacker.Status.MagicAttack - defender.Status.MagicDefense,
            DamageType.True => attacker.Status.Attack,
            _ => 0
        };

        //ダメージ倍率を適用
        damage *= damageMultiplier;

        if (damage < 1 && damageType != DamageType.True)
        {
            damage = 1;
        }

        bool isCritical = Random.value < attacker.Status.CriticalRate;

        if (isCritical)
        {
            damage *= 1 + (attacker.Status.CriticalDamage / 100f);
        }

        return new DamageResult(damage, isCritical, false);
    }
}