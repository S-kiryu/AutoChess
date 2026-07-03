using System.Collections.Generic;
using UnityEngine;

public class BattleMovementResolver : MonoBehaviour
{
    [SerializeField] private float resolveInterval = 0.1f;
    [SerializeField] private float moveRequestCooldown = 0.25f;

    private float resolveTimer;
    private bool isResolving;

    private Dictionary<BattleUnitBase, float> moveCooldowns =
        new Dictionary<BattleUnitBase, float>();

    public void StartResolve()
    {
        isResolving = true;
        resolveTimer = 0f;
        moveCooldowns.Clear();
    }

    public void StopResolve()
    {
        isResolving = false;
        moveCooldowns.Clear();
    }

    private void FixedUpdate()
    {
        if (!isResolving || BattleManager.Instance == null)
        {
            return;
        }

        UpdateCooldowns();

        resolveTimer -= Time.fixedDeltaTime;

        if (resolveTimer > 0f)
        {
            return;
        }

        resolveTimer = resolveInterval;
        ResolveOneMovement();
    }

    private void UpdateCooldowns()
    {
        if (moveCooldowns.Count == 0)
        {
            return;
        }

        List<BattleUnitBase> keys =
            new List<BattleUnitBase>(moveCooldowns.Keys);

        foreach (BattleUnitBase unit in keys)
        {
            moveCooldowns[unit] -= Time.fixedDeltaTime;

            if (moveCooldowns[unit] <= 0f)
            {
                moveCooldowns.Remove(unit);
            }
        }
    }

    private void ResolveOneMovement()
    {
        List<BattleUnitBase> units =
            BattleManager.Instance.GetAllBattleUnits();

        List<BattleUnitBase> candidates =
            new List<BattleUnitBase>();

        foreach (BattleUnitBase unit in units)
        {
            if (unit == null || !unit.CanRequestMove)
            {
                continue;
            }

            if (moveCooldowns.ContainsKey(unit))
            {
                continue;
            }

            BattleGrid nextGrid = unit.GetNextMoveGrid();

            if (nextGrid == null || nextGrid.IsEnterBlocked)
            {
                continue;
            }

            candidates.Add(unit);
        }

        if (candidates.Count == 0)
        {
            return;
        }

        candidates.Sort((a, b) =>
        {
            int speedCompare = b.Status.MoveSpeed.CompareTo(a.Status.MoveSpeed);

            if (speedCompare != 0)
            {
                return speedCompare;
            }

            return units.IndexOf(a).CompareTo(units.IndexOf(b));
        });

        BattleUnitBase selectedUnit = candidates[0];
        BattleGrid selectedGrid = selectedUnit.GetNextMoveGrid();

        if (selectedGrid == null || selectedGrid.IsEnterBlocked)
        {
            return;
        }

        if (!selectedGrid.TryLockForMove(selectedUnit))
        {
            return;
        }

        selectedUnit.BeginMoveTo(selectedGrid);
        moveCooldowns[selectedUnit] = moveRequestCooldown;
    }
}