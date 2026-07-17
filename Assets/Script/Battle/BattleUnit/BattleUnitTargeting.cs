using System.Collections.Generic;

public class BattleUnitTargeting
{
    private readonly BattleUnitBase owner;

    public BattleUnitTargeting(BattleUnitBase owner)
    {
        this.owner = owner;
    }

    public BattleUnitBase FindNearestEnemy()
    {
        if (BattleManager.Instance == null || owner.CurrentGrid == null)
            return null;

        IReadOnlyList<BattleUnitBase> enemies = BattleManager.Instance.GetEnemies(owner.Team);

        BattleUnitBase nearest = null;
        int nearestDistance = int.MaxValue;

        foreach (BattleUnitBase enemy in enemies)
        {
            if (enemy == null || enemy.IsDead || enemy.CurrentGrid == null)
                continue;

            int distance = BattlePathFinder.GetGridDistance(
                owner.CurrentGrid,
                enemy.CurrentGrid);

            if (distance < nearestDistance)
            {
                nearestDistance = distance;
                nearest = enemy;
            }
        }

        return nearest;
    }
}