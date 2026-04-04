using UnityEngine;
/// <summary>
///ダメージ計算を行うクラス
/// </summary>
public static class DamageCalculator
{
    public static float  CalculateDamage(AttackData attack, UnitModel target) 
    {
        float defense = (attack.Type == DamageType.Physical) 
            ? target.Defense 
            : target.MagicDefense;
        if(Random.value <  target.CriticalRate) // クリティカルヒットの判定
        {
            attack.Power *= 1.5f; // クリティカルヒットは1.5倍のダメージ
        }


        float finalDamage = attack.Power - defense;
        finalDamage = Mathf.Max(1, finalDamage);

        return finalDamage;
    }

    
}
