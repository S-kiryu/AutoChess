public class BattleUnitItemHandler
{
    private readonly BattleUnitBase owner;

    public BattleUnitItemHandler(BattleUnitBase owner)
    {
        this.owner = owner;
    }

    public void OnBattleStart()
    {
        foreach (ItemInstance item in owner.EquippedItems)
            item?.Data?.OnBattleStart(owner.Status);
    }

    public void OnNormalAttack(UnitStatus targetStatus)
    {
        foreach (ItemInstance item in owner.EquippedItems)
            item?.Data?.OnNormalAttack(owner.Status, targetStatus);
    }

    public void OnSkillUsed()
    {
        foreach (ItemInstance item in owner.EquippedItems)
            item?.Data?.OnSkillUsed(owner.Status);
    }

    public void OnOwnerDeath()
    {
        foreach (ItemInstance item in owner.EquippedItems)
            item?.Data?.OnOwnerDeath(owner.Status);
    }

    public void OnEndBattle()
    {
        foreach (ItemInstance item in owner.EquippedItems)
            item?.Data?.OnEndBattle(owner.Status);
    }
}