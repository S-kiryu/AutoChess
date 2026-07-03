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
        DamageType damageType)
    {
        if (attacker == null ||
            defender == null ||
            attacker.Status == null ||
            defender.Status == null)
        {
            return DamageResult.Zero;
        }

        bool isDodged = Random.value < defender.Status.DodgeRate;

        if (isDodged)
        {
            return new DamageResult(
                damage: 0,
                isCritical: false,
                isDodged: true);
        }

        float damage = GetBaseDamage(
            attacker,
            defender,
            damageType);

        if (damage < 1 && damageType != DamageType.True)
        {
            damage = 1;
        }

        bool isCritical =
            Random.value < attacker.Status.CriticalRate;

        if (isCritical)
        {
            damage *= 1+(attacker.Status.CriticalDamage/100);
        }

        return new DamageResult(
            damage,
            isCritical,
            isDodged: false);
    }

    /// <summary>
    /// どの種類の攻撃かを判定して防御力分引いてる
    /// </summary>
    private static float GetBaseDamage(
        BattleUnitBase attacker,
        BattleUnitBase defender,
        DamageType damageType)
    {
        switch (damageType)
        {
            case DamageType.Physical:
                return attacker.Status.Attack -
                       defender.Status.Defense;

            case DamageType.Magic:
                return attacker.Status.MagicAttack -
                       defender.Status.MagicDefense;

            case DamageType.True:
                return attacker.Status.Attack;

            default:
                return 0;
        }
    }
}