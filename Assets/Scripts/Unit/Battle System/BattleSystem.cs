using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BattleSystem : MonoBehaviour
{
    [SerializeField] private UnitManager unitManager;
    [SerializeField] private UnitPresenter unitPresenter;
    [SerializeField] private DragData dragData;
    [SerializeField] private float thinkInterval = 0.4f;

    private readonly Dictionary<UnitModel, float> _nextAttackTimes = new Dictionary<UnitModel, float>();

    private void Start()
    {
        StartCoroutine(BattleLoop());
    }

    //ゲームループ
    private IEnumerator BattleLoop()
    {
        while (true)
        {
            List<UnitModel> snapshot = unitManager.Units.ToList();

            //各ユニットの行動を処理
            foreach (UnitModel unit in snapshot)
            {
                if (unit == null || unit.CurrentHp <= 0)
                {
                    continue;
                }

                // ドラッグ中のユニットは行動しない
                if (dragData != null &&
                    dragData.IsDragging &&
                    dragData.CurrentView != null &&
                    dragData.CurrentView.Model == unit)
                {
                    continue;
                }

                Act(unit);
            }

            CleanupDeadUnits(snapshot);

            yield return new WaitForSeconds(thinkInterval);
        }
    }

    private void Act(UnitModel unit)
    {
        //最も近い敵を見つける
        UnitModel enemy = unitManager.FindNearestEnemy(unit);
        if (enemy == null)
        {
            return;
        }

        //攻撃範囲内にいるか見て、いれば攻撃、いなければ近づく
        if (IsInRange(unit, enemy))
        {
            TryAttack(unit, enemy);
            return;
        }

        MoveToward(unit, enemy);
    }

    private bool IsInRange(UnitModel attacker, UnitModel target)
    {
        int distance = GetGridDistance(attacker.GridPos, target.GridPos);
        return distance <= attacker.AttackRange;
    }

    private void TryAttack(UnitModel attacker, UnitModel target)
    {
        if (_nextAttackTimes.TryGetValue(attacker, out float nextAttackTime) &&
            Time.time < nextAttackTime)
        {
            return;
        }

        AttackData attackData = new AttackData(attacker.Attack, DamageType.Physical);
        float damage = DamageCalculator.CalculateDamage(attacker, attackData, target);
        target.TakeDamage(damage);
        unitPresenter.SyncUnitHP(target);

        float attackInterval = attacker.AttackSpeed <= 0 ? 1.0f : 1.0f / attacker.AttackSpeed;
        _nextAttackTimes[attacker] = Time.time + attackInterval;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="unit">行動するユニット</param>
    /// <param name="target">狙うユニット</param>
    private void MoveToward(UnitModel unit, UnitModel target)
    {
        Vector2Int currentPos = unit.GridPos;
        List<Vector2Int> candidates = GetCandidateSteps(currentPos, target.GridPos);

        foreach (Vector2Int nextPos in candidates)
        {
            if (unitManager.MoveUnit(unit, nextPos))
            {
                unitPresenter.SyncUnitPosition(unit);
                return;
            }
        }
    }

    private List<Vector2Int> GetCandidateSteps(Vector2Int currentPos, Vector2Int targetPos)
    {
        List<Vector2Int> candidates = new List<Vector2Int>();
        Vector2Int diff = targetPos - currentPos;

        if (Mathf.Abs(diff.x) >= Mathf.Abs(diff.y) && diff.x != 0)
        {
            candidates.Add(currentPos + new Vector2Int(diff.x > 0 ? 1 : -1, 0));
        }

        if (diff.y != 0)
        {
            candidates.Add(currentPos + new Vector2Int(0, diff.y > 0 ? 1 : -1));
        }

        if (Mathf.Abs(diff.x) < Mathf.Abs(diff.y) && diff.x != 0)
        {
            candidates.Add(currentPos + new Vector2Int(diff.x > 0 ? 1 : -1, 0));
        }

        return candidates.Distinct().ToList();
    }

    private int GetGridDistance(Vector2Int a, Vector2Int b)
    {
        return Mathf.Abs(a.x - b.x) + Mathf.Abs(a.y - b.y);
    }

    private void CleanupDeadUnits(List<UnitModel> units)
    {
        foreach (UnitModel unit in units)
        {
            if (unit == null || unit.CurrentHp > 0)
            {
                continue;
            }

            unitPresenter.RemoveUnit(unit);
            _nextAttackTimes.Remove(unit);
        }
    }
}
