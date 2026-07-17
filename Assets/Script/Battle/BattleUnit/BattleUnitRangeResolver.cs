using System.Collections.Generic;
using UnityEngine;

public class BattleUnitRangeResolver
{
    private readonly BattleUnitBase owner;

    public BattleUnitRangeResolver(BattleUnitBase owner)
    {
        this.owner = owner;
    }

    public List<BattleGrid> GetTargetGrids(AttackActionData actionData)
    {
        List<BattleGrid> grids = new List<BattleGrid>();

        if (actionData == null || actionData.RangeData == null)
        {
            if (owner.Target != null && owner.Target.CurrentGrid != null)
                grids.Add(owner.Target.CurrentGrid);

            return grids;
        }

        BattleGrid originGrid = GetRangeOriginGrid(actionData);

        if (originGrid == null || actionData.RangeData.Offsets == null)
            return grids;

        foreach (Vector2Int offset in actionData.RangeData.Offsets)
        {
            Vector2Int worldOffset = offset;

            if (actionData.RangeData.Origin == ActionRangeOrigin.FrontOfSelf)
                worldOffset = RotateOffsetByForward(offset);

            BattleGrid grid = BattleGridManager.Instance.GetGridByBoardPosition(
                originGrid.BoardX + worldOffset.x,
                originGrid.BoardY + worldOffset.y);

            if (grid != null)
                grids.Add(grid);
        }

        return grids;
    }

    public List<BattleUnitBase> GetHitUnits(List<BattleGrid> targetGrids)
    {
        List<BattleUnitBase> hitUnits = new List<BattleUnitBase>();

        if (targetGrids == null)
            return hitUnits;

        foreach (BattleGrid grid in targetGrids)
        {
            if (grid == null || grid.CurrentBattleUnit == null)
                continue;

            BattleUnitBase hitUnit = grid.CurrentBattleUnit;

            if (hitUnit.Team == owner.Team)
                continue;

            if (!hitUnits.Contains(hitUnit))
                hitUnits.Add(hitUnit);
        }

        return hitUnits;
    }

    private BattleGrid GetRangeOriginGrid(AttackActionData actionData)
    {
        switch (actionData.RangeData.Origin)
        {
            case ActionRangeOrigin.Target:
                return owner.Target != null ? owner.Target.CurrentGrid : null;

            case ActionRangeOrigin.Self:
                return owner.CurrentGrid;

            case ActionRangeOrigin.FrontOfSelf:
                return GetFrontGrid();

            default:
                return null;
        }
    }

    private BattleGrid GetFrontGrid()
    {
        if (owner.CurrentGrid == null || BattleGridManager.Instance == null)
            return null;

        return BattleGridManager.Instance.GetGridByBoardPosition(
            owner.CurrentGrid.BoardX + owner.ForwardDirection.x,
            owner.CurrentGrid.BoardY + owner.ForwardDirection.y);
    }

    private Vector2Int RotateOffsetByForward(Vector2Int offset)
    {
        Vector2Int forward = owner.ForwardDirection;

        if (forward == Vector2Int.up)
            return offset;

        if (forward == Vector2Int.down)
            return new Vector2Int(-offset.x, -offset.y);

        if (forward == Vector2Int.right)
            return new Vector2Int(offset.y, -offset.x);

        if (forward == Vector2Int.left)
            return new Vector2Int(-offset.y, offset.x);

        return offset;
    }
}