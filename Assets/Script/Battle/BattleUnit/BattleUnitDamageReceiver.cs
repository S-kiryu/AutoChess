using UnityEngine;

public class BattleUnitDamageReceiver
{
    private readonly BattleUnitBase owner;
    private readonly DamagePopupManager damagePopupManager;
    private readonly BattleUnitView unitView;
    private readonly BattleUnitItemHandler itemHandler;
    private readonly int takeDamageManaGain;

    public BattleUnitDamageReceiver(
        BattleUnitBase owner,
        DamagePopupManager damagePopupManager,
        BattleUnitView unitView,
        BattleUnitItemHandler itemHandler,
        int takeDamageManaGain)
    {
        this.owner = owner;
        this.damagePopupManager = damagePopupManager;
        this.unitView = unitView;
        this.itemHandler = itemHandler;
        this.takeDamageManaGain = takeDamageManaGain;
    }

    public void TakeDamage(float damage)
    {
        if (owner.Status == null || owner.IsDead)
            return;

        int beforeMp = owner.Status.CurrentMp;

        owner.Status.TakeDamage(damage);
        owner.Status.AddMana(takeDamageManaGain);

        if (damagePopupManager != null)
            damagePopupManager.ShowDamage(Mathf.RoundToInt(damage), owner.transform);

        Debug.Log(
            $"{owner.UnitInstance.Data.CharacterName} took {damage} damage. " +
            $"HP: {owner.Status.CurrentHp}/{owner.Status.MaxHp}, " +
            $"MP: {beforeMp} -> {owner.Status.CurrentMp}/{owner.Status.MaxMp}");

        if (unitView != null)
            unitView.PlayDamageFlash();

        if (owner.Status.IsDead)
        {
            itemHandler.OnOwnerDeath();
            owner.DieFromDamage();
        }
    }
}