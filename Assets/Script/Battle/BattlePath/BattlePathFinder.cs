using System.Collections.Generic;
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

    public static List<BattleGrid> GetNeighborGrids(BattleGrid grid)
    {
        List<BattleGrid> neighbors = new List<BattleGrid>();

        if (grid == null || BattleGridManager.Instance == null)
        {
            return neighbors;
        }

        AddNeighbor(neighbors, grid.BoardX + 1, grid.BoardY);
        AddNeighbor(neighbors, grid.BoardX - 1, grid.BoardY);
        AddNeighbor(neighbors, grid.BoardX, grid.BoardY + 1);
        AddNeighbor(neighbors, grid.BoardX, grid.BoardY - 1);

        return neighbors;
    }

    private static void AddNeighbor(
        List<BattleGrid> neighbors,
        int boardX,
        int boardY)
    {
        BattleGrid grid =
            BattleGridManager.Instance.GetGridByBoardPosition(
                boardX,
                boardY);

        if (grid != null)
        {
            neighbors.Add(grid);
        }
    }

    public static BattleGrid GetNextGridTowardForwardAttackRange(
        BattleGrid from,
        BattleGrid target,
        BattleTeam team,
        int width,
        int depth)
    {
        if (from == null || target == null)
        {
            return null;
        }

        Queue<BattleGrid> open = new Queue<BattleGrid>();
        Dictionary<BattleGrid, BattleGrid> cameFrom =
            new Dictionary<BattleGrid, BattleGrid>();

        open.Enqueue(from);
        cameFrom[from] = null;

        BattleGrid bestGrid = null;
        int bestDistance = int.MaxValue;

        while (open.Count > 0)
        {
            BattleGrid current = open.Dequeue();

            if (IsGridInForwardAttackRange(
                    current,
                    target,
                    team,
                    width,
                    depth))
            {
                bestGrid = current;
                break;
            }

            int distanceToTarget = GetGridDistance(current, target);

            if (distanceToTarget < bestDistance)
            {
                bestDistance = distanceToTarget;
                bestGrid = current;
            }

            foreach (BattleGrid neighbor in GetNeighborGrids(current))
            {
                if (cameFrom.ContainsKey(neighbor))
                {
                    continue;
                }

                if (neighbor.HasBattleUnit)
                {
                    continue;
                }

                cameFrom[neighbor] = current;
                open.Enqueue(neighbor);
            }
        }

        if (bestGrid == null || bestGrid == from)
        {
            return null;
        }

        BattleGrid step = bestGrid;

        while (cameFrom[step] != from)
        {
            step = cameFrom[step];
        }

        return step;
    }

    public static bool IsGridInForwardAttackRange(
        BattleGrid attackerGrid,
        BattleGrid targetGrid,
        BattleTeam team,
        int width,
        int depth)
    {
        if (attackerGrid == null || targetGrid == null)
        {
            return false;
        }

        int dx = targetGrid.BoardX - attackerGrid.BoardX;
        int dy = targetGrid.BoardY - attackerGrid.BoardY;

        int forwardDistance =
            team == BattleTeam.Player ? -dy : dy;

        if (forwardDistance <= 0)
        {
            return false;
        }

        int halfWidth = width / 2;

        return Mathf.Abs(dx) <= halfWidth &&
               forwardDistance <= depth;
    }
}