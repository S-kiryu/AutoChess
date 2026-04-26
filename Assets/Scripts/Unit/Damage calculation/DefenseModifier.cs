public class DefenseModifier : IDamageModifier
{
    public void Apply(DamageContext context)
    {
        float defense = context.Attack.Type == DamageType.Physical
            ? context.Target.Defense
            : context.Target.MagicDefense;

        context.ReducedDamage = context.ModifiedDamage - defense;
    }
}
