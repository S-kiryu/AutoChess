using UnityEngine;

[CreateAssetMenu(menuName = "AutoChess/SkillEffect/Damage")]
public class DamageSkillEffectData : SkillEffectData
{
    [SerializeField] private DamageType damageType = DamageType.Magic;
    [SerializeField] private float damageMultiplier = 1f;

    public override void Apply(SkillEffectContext context)
    {
        foreach (BattleUnitBase unit in context.HitUnits)
        {
            if (unit == null || unit.IsDead)
            {
                continue;
            }

            DamageResult result = DamageCalculator.CalculateDamage(
                context.Caster,
                unit,
                damageType,
                damageMultiplier);

            if (!result.IsDodged)
            {
                Debug.Log($"{context.Caster.UnitInstance.Data.name}が{unit.UnitInstance.Data.name}に{result.Damage}を与えた");
                unit.TakeDamage(result.Damage);
            }
        }
    }
}