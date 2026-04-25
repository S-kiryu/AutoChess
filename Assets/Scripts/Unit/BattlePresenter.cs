using UnityEngine;

public class BattlePresenter
{
    [SerializeField] private DragData dragData;

    private UnitModel attacker;
    private UnitModel target;

    private UnitView attackerView;
    private UnitView targetView;

    public BattlePresenter(
        UnitModel attacker,
        UnitModel target,
        UnitView attackerView,
        UnitView targetView)
    {
        this.attacker = attacker;
        this.target = target;
        this.attackerView = attackerView;
        this.targetView = targetView;

        UpdateView();
    }

    public void Attack()
    {
        var attackData = new AttackData(attacker.Attack, DamageType.Physical);

        float damage = DamageCalculator.CalculateDamage(attacker, attackData, target);

        target.TakeDamage(damage);

        UpdateView();
    }

    //Uiの更新を行うメソッド
    private void UpdateView()
    {
        attackerView.SetHP(attacker.CurrentHp);
        targetView.SetHP(target.CurrentHp);
    }
}