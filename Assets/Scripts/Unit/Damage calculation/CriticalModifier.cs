using UnityEngine;
public class CriticalModifier : IDamageModifier
{
    public void Apply(DamageContext context)
    {
        context.ModifiedDamage = context.BaseDamage;

        if (Random.value < context.Attacker.CriticalRate)
        {
            context.IsCritical = true;
            context.ModifiedDamage *= context.Attacker.CriticalDamage;
        }
    }
}