using System.Collections.Generic;

public static class DamageCalculator
{
    private static readonly List<IDamageModifier> modifiers = new()
    {
        new CriticalModifier(),
        new DefenseModifier(),
        new MinDamageModifier()
    };

    public static float CalculateDamage(UnitModel attacker, AttackData attack, UnitModel target)
    {
        var context = new DamageContext
        {
            Attacker = attacker,
            Target = target,
            Attack = attack,
            Power = attack.Power
        };

        foreach (var modifier in modifiers)
        {
            modifier.Apply(context);
        }

        return context.FinalDamage;
    }
}