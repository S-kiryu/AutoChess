using UnityEngine;

public static class BattlePathFinder
{
    public static int GetGridDistance(BattleGrid from, BattleGrid to)
    {
        if (from == null || to == null)
        {
            return int.MaxValue;
        }

        return Mathf.Abs(from.BoardX - to.BoardX) +
               Mathf.Abs(from.BoardY - to.BoardY);
    }

    public static BattleGrid GetNextGridTowardTarget(
        BattleGrid from,
        BattleGrid target)
    {
        if (from == null || target == null || BattleGridManager.Instance == null)
        {
            return null;
        }

        int dx = target.BoardX - from.BoardX;
        int dy = target.BoardY - from.BoardY;

        BattleGrid nextGrid = null;

        if (Mathf.Abs(dy) >= Mathf.Abs(dx))
        {
            if (dy != 0)
            {
                nextGrid = BattleGridManager.Instance.GetGridByBoardPosition(
                    from.BoardX,
                    from.BoardY + (dy > 0 ? 1 : -1));
            }
        }
        else
        {
            if (dx != 0)
            {
                nextGrid = BattleGridManager.Instance.GetGridByBoardPosition(
                    from.BoardX + (dx > 0 ? 1 : -1),
                    from.BoardY);
            }
        }

        if (nextGrid == null || nextGrid.IsEnterBlocked)
        {
            return null;
        }

        return nextGrid;
    }
}