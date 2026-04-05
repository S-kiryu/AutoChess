using UnityEngine;
/// <summary>
///ダメージ計算を行うクラス
/// </summary>
public static class DamageCalculator
{
    public static float CalculateDamage(UnitModel attacker, AttackData attack, UnitModel target)
    {
        float power = attack.Power;

        // クリティカル
        if (Random.value < attacker.CriticalRate)
        {
            power *= attacker.CriticalDamage;
        }

        // 防御判定
        float defense = (attack.Type == DamageType.Physical)
            ? target.Defense
            : target.MagicDefense;

        float finalDamage = power - defense;
        finalDamage = Mathf.Max(1, finalDamage);

        return finalDamage;
    }
}
