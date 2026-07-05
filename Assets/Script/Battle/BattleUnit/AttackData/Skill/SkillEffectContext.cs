using System.Collections.Generic;

public class SkillEffectContext
{
    public BattleUnitBase Caster;
    public BattleUnitBase MainTarget;
    public AttackActionData ActionData;
    public List<BattleGrid> TargetGrids;
    public List<BattleUnitBase> HitUnits;
}