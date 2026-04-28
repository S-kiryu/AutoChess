using UnityEngine;
//Å’á•ÛØ‚Ìƒ_ƒ[ƒW‚ğ1‚É‚·‚é
public class MinDamageModifier : IDamageModifier
{
    public void Apply(DamageContext context)
    {
        context.FinalDamage = Mathf.Max(1, context.ReducedDamage);
    }
}
