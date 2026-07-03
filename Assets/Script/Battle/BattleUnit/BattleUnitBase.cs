using UnityEngine;
using UnityEngine.UI;
using System.Collections;

/// <summary>
/// 戦闘時のデータ
/// </summary>
public class BattleUnitBase : MonoBehaviour
{
    [SerializeField] private BattleUnitView unitView;

    public UnitInstance UnitInstance { get; private set; }

    public BattleGrid CurrentGrid { get; private set; }
    public UnitStatus Status { get; private set; }

    public BattleTeam Team { get; private set; }
    public bool IsDead => Status != null && Status.CurrentHp <= 0;

    private BattleUnitBase target;
    private float attackTimer;
    private bool isBattling;
    private bool isMoving;
    private Vector3 moveDestination;
    private Vector2Int forwardDirection = Vector2Int.up;

    public void Initialize(UnitInstance unitInstance, BattleTeam teamId)
    {
        UnitInstance = unitInstance;
        Status = unitInstance.Status;
        Team = teamId;

        attackTimer = Status.AttackSpeed;

        if (unitView == null)
        {
            unitView = GetComponent<BattleUnitView>();
        }

        if (unitView != null)
        {
            unitView.SetUnit(UnitInstance);
        }

        if (BattleManager.Instance != null)
        {
            BattleManager.Instance.RegisterUnit(this);
        }
    }

    public void SetCurrentGrid(BattleGrid grid)
    {
        if (CurrentGrid != null)
        {
            CurrentGrid.ClearBattleUnit(this);
        }

        CurrentGrid = grid;

        if (CurrentGrid != null)
        {
            CurrentGrid.SetBattleUnit(this);
            transform.position = CurrentGrid.transform.position;
        }
    }

    public void StartBattle()
    {
        Debug.Log($"StartBattle: {name}, Team: {Team}");

        isBattling = true;
        target = null;
    }

    public void StopBattle()
    {
        isBattling = false;
    }

    private void FixedUpdate()
    {
        if (!isBattling || Status == null || IsDead)
        {
            return;
        }

        if (isMoving)
        {
            ContinueMove();
            return;
        }

        if (target == null || target.IsDead)
        {
            FindNearestEnemy();
        }

        if (target == null)
        {
            return;
        }

        AttackRangeCheck();
    }

    public void SetTarget(BattleUnitBase newTarget)
    {
        target = newTarget;
    }

    // 一番近い敵を見つける
    private void FindNearestEnemy()
    {
        if (BattleManager.Instance == null || CurrentGrid == null)
        {
            return;
        }

        var enemies = BattleManager.Instance.GetEnemies(Team);

        BattleUnitBase nearest = null;
        int nearestDistance = int.MaxValue;

        foreach (BattleUnitBase enemy in enemies)
        {
            if (enemy == null || enemy.IsDead || enemy.CurrentGrid == null)
            {
                continue;
            }

            int distance = BattlePathFinder.GetGridDistance(
                CurrentGrid,
                enemy.CurrentGrid);

            if (distance < nearestDistance)
            {
                nearestDistance = distance;
                nearest = enemy;
            }
        }

        target = nearest;
    }

    // 攻撃が届くかどうかを確認して行動を決める
    public void AttackRangeCheck()
    {
        if (target == null)
        {
            return;
        }

        NormalAttackData attackData = UnitInstance.Data.NormalAttack;

        if (attackData == null)
        {
            Debug.LogWarning($"{name} に通常攻撃データがありません。");
            return;
        }

        FaceTarget();

        if (IsTargetInForwardAttackRange(target, attackData))
        {
            Attack(attackData);
        }
        else
        {
            MoveToTarget(attackData);
        }
    }

    private void MoveToTarget(NormalAttackData attackData)
    {
        if (target == null ||
            target.CurrentGrid == null ||
            CurrentGrid == null ||
            attackData == null)
        {
            return;
        }

        BattleGrid nextGrid =
            BattlePathFinder.GetNextGridTowardForwardAttackRange(
                CurrentGrid,
                target.CurrentGrid,
                Team,
                attackData.Width,
                attackData.Depth);

        if (nextGrid == null)
        {
            return;
        }

        FaceGrid(nextGrid);

        CurrentGrid.ClearBattleUnit(this);

        CurrentGrid = nextGrid;
        CurrentGrid.SetBattleUnit(this);

        moveDestination = CurrentGrid.transform.position;
        isMoving = true;
    }

    private void ContinueMove()
    {
        transform.position = Vector3.MoveTowards(
            transform.position,
            moveDestination,
            Status.MoveSpeed * Time.fixedDeltaTime);

        if (Vector3.Distance(transform.position, moveDestination) <= 0.01f)
        {
            transform.position = moveDestination;
            isMoving = false;
        }
    }

    private void Attack(NormalAttackData attackData)
    {
        attackTimer -= Time.fixedDeltaTime;

        if (attackTimer > 0)
        {
            return;
        }

        DamageResult result = DamageCalculator.CalculateDamage(
            this,
            target,
            attackData.DamageType,
            attackData.DamageMultiplier);

        if (result.IsDodged)
        {
            Debug.Log($"{target.name} が回避しました。");
            attackTimer = Status.AttackSpeed;
            return;
        }

        target.TakeDamage(result.Damage);
        Debug.Log($"{name} が {target.name} に {result.Damage} のダメージを与えました。");

        attackTimer = Status.AttackSpeed;
    }

    private bool IsTargetInForwardAttackRange(
    BattleUnitBase targetUnit,
    NormalAttackData attackData)
    {
        if (targetUnit == null ||
            targetUnit.CurrentGrid == null ||
            CurrentGrid == null ||
            attackData == null)
        {
            return false;
        }

        int dx = targetUnit.CurrentGrid.BoardX - CurrentGrid.BoardX;
        int dy = targetUnit.CurrentGrid.BoardY - CurrentGrid.BoardY;

        int forwardDistance =
            dx * forwardDirection.x +
            dy * forwardDirection.y;

        if (forwardDistance <= 0)
        {
            return false;
        }

        int sideDistance =
            Mathf.Abs(
                dx * -forwardDirection.y +
                dy * forwardDirection.x);

        int halfWidth = attackData.Width / 2;

        return sideDistance <= halfWidth &&
               forwardDistance <= attackData.Depth;
    }

    public void TakeDamage(float damage)
    {
        if (Status == null || IsDead)
        {
            return;
        }

        Status.TakeDamage(damage);

        if (unitView != null)
        {
            unitView.PlayDamageFlash();
        }

        if (Status.IsDead)
        {
            Die();
        }
    }

    private void Die()
    {
        isBattling = false;
        isMoving = false;

        if (unitView != null)
        {
            unitView.ResetColor();
        }

        if (CurrentGrid != null)
        {
            CurrentGrid.ClearBattleUnit(this);
        }

        gameObject.SetActive(false);

        if (BattleManager.Instance != null)
        {
            BattleManager.Instance.NotifyUnitDead(this);
        }
    }

    private void FaceTarget()
    {
        if (target == null ||
            target.CurrentGrid == null ||
            CurrentGrid == null)
        {
            return;
        }

        int dx = target.CurrentGrid.BoardX - CurrentGrid.BoardX;
        int dy = target.CurrentGrid.BoardY - CurrentGrid.BoardY;

        if (Mathf.Abs(dx) > Mathf.Abs(dy))
        {
            forwardDirection = dx > 0
                ? Vector2Int.right
                : Vector2Int.left;
        }
        else
        {
            forwardDirection = dy > 0
                ? Vector2Int.up
                : Vector2Int.down;
        }
    }

    private void FaceGrid(BattleGrid grid)
    {
        if (grid == null || CurrentGrid == null)
        {
            return;
        }

        int dx = grid.BoardX - CurrentGrid.BoardX;
        int dy = grid.BoardY - CurrentGrid.BoardY;

        if (dx == 0 && dy == 0)
        {
            return;
        }

        if (Mathf.Abs(dx) > Mathf.Abs(dy))
        {
            forwardDirection = dx > 0
                ? Vector2Int.right
                : Vector2Int.left;
        }
        else
        {
            forwardDirection = dy > 0
                ? Vector2Int.up
                : Vector2Int.down;
        }
    }
}

public enum BattleTeam
{
    Player,
    Enemy
}