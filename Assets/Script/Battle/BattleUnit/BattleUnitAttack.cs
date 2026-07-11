using System.Collections.Generic;
using UnityEngine;

public class BattleUnitAttack
{
    private readonly BattleUnitBase owner;
    private readonly BattleActionRangeVisualizer rangeVisualizer;
    private readonly BattleUnitRangeResolver rangeResolver;
    private readonly BattleUnitItemHandler itemHandler;
    private readonly int normalAttackManaGain;

    private float attackTimer;

    public BattleUnitAttack(
        BattleUnitBase owner,
        BattleActionRangeVisualizer rangeVisualizer,
        BattleUnitRangeResolver rangeResolver,
        BattleUnitItemHandler itemHandler,
        int normalAttackManaGain)
    {
        this.owner = owner;
        this.rangeVisualizer = rangeVisualizer;
        this.rangeResolver = rangeResolver;
        this.itemHandler = itemHandler;
        this.normalAttackManaGain = normalAttackManaGain;
    }

    public void ResetTimer()
    {
        attackTimer = 0f;
    }

    public void Tick()
    {
        attackTimer -= Time.fixedDeltaTime;

        if (attackTimer > 0)
            return;

        SkillData skill = owner.UnitInstance.Data.Skill;

        if (skill != null &&
            owner.Status.CurrentMp >= skill.ManaCost &&
            owner.CanUseAction(skill))
        {
            owner.Status.ConsumeAllMana();
            itemHandler.OnSkillUsed();

            ExecuteAction(skill);

            attackTimer = owner.Status.AttackSpeed;
            return;
        }

        NormalAttackData normalAttack = owner.UnitInstance.Data.NormalAttack;

        if (normalAttack != null && owner.CanUseAction(normalAttack))
        {
            ExecuteAction(normalAttack);

            owner.Status.AddMana(normalAttackManaGain);
            attackTimer = owner.Status.AttackSpeed;
        }
    }

    private void ExecuteAction(AttackActionData actionData)
    {
        List<BattleGrid> targetGrids = rangeResolver.GetTargetGrids(actionData);

        if (rangeVisualizer != null)
            rangeVisualizer.FlashRange(actionData, targetGrids);

        List<BattleUnitBase> hitUnits = rangeResolver.GetHitUnits(targetGrids);

        SkillEffectContext context = new SkillEffectContext
        {
            Caster = owner,
            MainTarget = owner.Target,
            ActionData = actionData,
            TargetGrids = targetGrids,
            HitUnits = hitUnits
        };

        SkillData skill = actionData as SkillData;

        if (skill != null && skill.Effects != null && skill.Effects.Length > 0)
        {
            foreach (SkillEffectData effect in skill.Effects)
            {
                if (effect == null)
                    continue;

                effect.Apply(context);
            }

            return;
        }

        ExecuteDamageAction(actionData, hitUnits);
    }

    private void ExecuteDamageAction(
        AttackActionData actionData,
        List<BattleUnitBase> hitUnits)
    {
        int hitCount = Mathf.Max(1, actionData.HitCount);

        for (int i = 0; i < hitCount; i++)
        {
            foreach (BattleUnitBase hitUnit in hitUnits)
            {
                if (hitUnit == null || hitUnit.IsDead)
                    continue;

                DamageResult result = DamageCalculator.CalculateDamage(
                    owner,
                    hitUnit,
                    actionData.DamageType,
                    actionData.DamageMultiplier);

                if (result.IsDodged)
                    continue;

                hitUnit.TakeDamage(result.Damage);
                itemHandler.OnNormalAttack(hitUnit.Status);
            }
        }
    }
}