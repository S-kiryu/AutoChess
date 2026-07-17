using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// グリットの移動の計算をするクラス
/// </summary>
public static class BattlePathFinder
{
    /// <summary>
    /// グリット間の距離を計算する
    /// </summary>
    /// <param name="from"></param>
    /// <param name="to"></param>
    /// <returns></returns>
    public static int GetGridDistance(BattleGrid from, BattleGrid to)
    {
        if (from == null || to == null)
        {
            return int.MaxValue;
        }

        return Mathf.Abs(from.BoardX - to.BoardX) +
               Mathf.Abs(from.BoardY - to.BoardY);
    }

    private static List<BattleGrid> GetNeighbors(
    BattleGrid grid,
    BattleGrid target)
    {
        List<BattleGrid> neighbors = new List<BattleGrid>();

        AddNeighbor(neighbors, grid.BoardX, grid.BoardY + 1);
        AddNeighbor(neighbors, grid.BoardX, grid.BoardY - 1);
        AddNeighbor(neighbors, grid.BoardX - 1, grid.BoardY);
        AddNeighbor(neighbors, grid.BoardX + 1, grid.BoardY);

        neighbors.Sort((a, b) =>
            GetGridDistance(a, target).CompareTo(GetGridDistance(b, target)));

        return neighbors;
    }

    private static void AddNeighbor(
        List<BattleGrid> neighbors,
        int boardX,
        int boardY)
    {
        BattleGrid grid =
            BattleGridManager.Instance.GetGridByBoardPosition(boardX, boardY);

        if (grid != null)
        {
            neighbors.Add(grid);
        }
    }

    /// <summary>
    /// 止まっているターゲットに対して、攻撃できる位置まで回り込む次のグリッドを取得する
    /// </summary>
    public static BattleGrid GetNextGridTowardStoppedTarget(
        BattleGrid from,
        BattleGrid target,
        int castRange)
    {
        if (from == null || target == null || BattleGridManager.Instance == null)
        {
            return null;
        }

        int range = Mathf.Max(1, castRange);

        if (GetGridDistance(from, target) <= range)
        {
            return null;
        }

        Queue<BattleGrid> open = new Queue<BattleGrid>();
        HashSet<BattleGrid> visited = new HashSet<BattleGrid>();
        Dictionary<BattleGrid, BattleGrid> previous =
            new Dictionary<BattleGrid, BattleGrid>();

        open.Enqueue(from);
        visited.Add(from);

        BattleGrid bestGrid = null;

        while (open.Count > 0)
        {
            BattleGrid current = open.Dequeue();

            if (current != target &&
                GetGridDistance(current, target) <= range)
            {
                bestGrid = current;
                break;
            }

            foreach (BattleGrid neighbor in GetNeighbors(current, target))
            {
                if (neighbor == null || visited.Contains(neighbor))
                {
                    continue;
                }

                if (neighbor == target)
                {
                    continue;
                }

                if (neighbor != from && neighbor.IsEnterBlocked)
                {
                    continue;
                }

                visited.Add(neighbor);
                previous[neighbor] = current;
                open.Enqueue(neighbor);
            }
        }

        if (bestGrid == null)
        {
            return null;
        }

        BattleGrid nextGrid = bestGrid;

        while (previous.TryGetValue(nextGrid, out BattleGrid prev) &&
               prev != from)
        {
            nextGrid = prev;
        }

        if (nextGrid == null || nextGrid.IsEnterBlocked)
        {
            return null;
        }

        return nextGrid;
    }

    /// <summary>
    /// ターゲットに向かって次のグリッドを取得する
    /// </summary>  
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