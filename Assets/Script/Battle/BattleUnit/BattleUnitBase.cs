using System.Collections.Generic;
using UnityEngine;

public class BattleUnitBase : MonoBehaviour
{
    [SerializeField] private BattleUnitView unitView;
    [SerializeField] private int chatterLimit = 2;
    [SerializeField] private float chatterStopTime = 0.5f;

    public UnitInstance UnitInstance { get; private set; }
    public BattleGrid CurrentGrid { get; private set; }
    public UnitStatus Status { get; private set; }
    public BattleTeam Team { get; private set; }

    public bool IsDead => Status != null && Status.CurrentHp <= 0;
    public bool IsMoving => isMoving;

    private BattleUnitBase target;
    private BattleGrid moveTargetGrid;
    private BattleGrid previousGrid;
    private List<BattleGrid> currentPath = new List<BattleGrid>();

    private int chatterCount;
    private float moveStopTimer;
    private float attackTimer;
    private bool isBattling;
    private bool isMoving;
    private Vector3 moveDestination;
    private Vector2Int forwardDirection = Vector2Int.up;

    public bool CanRequestMove
    {
        get
        {
            if (moveStopTimer > 0f)
            {
                return false;
            }

            if (!isBattling ||
                isMoving ||
                Status == null ||
                IsDead ||
                target == null ||
                target.CurrentGrid == null ||
                CurrentGrid == null)
            {
                return false;
            }

            NormalAttackData attackData = UnitInstance.Data.NormalAttack;

            if (attackData == null)
            {
                return false;
            }

            if (IsEngagedWithTarget() || CanAttackTarget(attackData))
            {
                return false;
            }

            return true;
        }
    }

    public void Initialize(UnitInstance unitInstance, BattleTeam teamId)
    {
        UnitInstance = unitInstance;
        Status = unitInstance.Status;
        Team = teamId;

        attackTimer = 0f;

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

    /// <summary>
    /// 現在のユニットを登録する関数
    /// </summary>
    /// <param name="grid"></param>
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
        if (moveTargetGrid != null)
        {
            moveTargetGrid.ClearMoveLock(this);
            moveTargetGrid = null;
        }

        isBattling = true;
        isMoving = false;
        target = null;
        currentPath.Clear();
    }

    public void StopBattle()
    {
        isBattling = false;
        isMoving = false;
        currentPath.Clear();

        previousGrid = null;
        chatterCount = 0;
        moveStopTimer = 0f;

        if (moveTargetGrid != null)
        {
            moveTargetGrid.ClearMoveLock(this);
            moveTargetGrid = null;
        }
    }

    private void FixedUpdate()
    {
        if (moveStopTimer > 0f)
        {
            moveStopTimer -= Time.fixedDeltaTime;
        }

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
            currentPath.Clear();
            FindNearestEnemy();
        }

        if (target == null)
        {
            return;
        }

        AttackRangeCheck();
    }

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

    public void AttackRangeCheck()
    {
        if (target == null || UnitInstance == null || UnitInstance.Data == null)
        {
            return;
        }

        NormalAttackData attackData = UnitInstance.Data.NormalAttack;

        if (attackData == null)
        {
            return;
        }

        FaceTarget();

        if (IsEngagedWithTarget() || CanAttackTarget(attackData))
        {
            currentPath.Clear();
            Attack(attackData);
        }
    }

    public BattleGrid GetNextMoveGrid()
    {
        if (!CanRequestMove || UnitInstance == null || UnitInstance.Data == null)
        {
            return null;
        }

        NormalAttackData attackData = UnitInstance.Data.NormalAttack;

        if (attackData == null)
        {
            return null;
        }

        FaceTarget();

        if (IsEngagedWithTarget() || CanAttackTarget(attackData))
        {
            currentPath.Clear();
            return null;
        }


        BattleGrid nextGrid = BattlePathFinder.GetNextGridTowardTarget(
            CurrentGrid,
            target.CurrentGrid);

        if (nextGrid == null)
        {
            return null;
        }

        if (nextGrid == target.CurrentGrid)
        {
            return null;
        }

        return nextGrid;
    }


    public void BeginMoveTo(BattleGrid nextGrid)
    {
        if (nextGrid == null || isMoving)
        {
            return;
        }

        if (previousGrid != null && nextGrid == previousGrid)
        {
            chatterCount++;

            if (chatterCount >= chatterLimit)
            {
                moveStopTimer = chatterStopTime;
                chatterCount = 0;
                currentPath.Clear();
                return;
            }
        }
        else
        {
            chatterCount = 0;
        }

        FaceGrid(nextGrid);

        moveTargetGrid = nextGrid;
        moveDestination = moveTargetGrid.transform.position;
        isMoving = true;
    }

    private void ContinueMove()
    {
        transform.position = Vector3.MoveTowards(
            transform.position,
            moveDestination,
            Status.MoveSpeed * Time.fixedDeltaTime);

        if (Vector3.Distance(transform.position, moveDestination) > 0.01f)
        {
            return;
        }

        if (moveTargetGrid == null)
        {
            isMoving = false;
            return;
        }

        transform.position = moveTargetGrid.transform.position;

        BattleGrid oldGrid = CurrentGrid;

        if (CurrentGrid != null)
        {
            CurrentGrid.ClearBattleUnit(this);
        }

        moveTargetGrid.ClearMoveLock(this);

        CurrentGrid = moveTargetGrid;
        CurrentGrid.SetBattleUnit(this);

        previousGrid = oldGrid;

        moveTargetGrid = null;
        isMoving = false;
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
            attackTimer = Status.AttackSpeed;
            return;
        }

        target.TakeDamage(result.Damage);
        attackTimer = Status.AttackSpeed;
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
        currentPath.Clear();

        if (moveTargetGrid != null)
        {
            moveTargetGrid.ClearMoveLock(this);
            moveTargetGrid = null;
        }

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

    private bool CanAttackTarget(NormalAttackData attackData)
    {
        if (target == null ||
            target.CurrentGrid == null ||
            CurrentGrid == null ||
            attackData == null)
        {
            return false;
        }

        int attackRange = Mathf.Max(1, attackData.Depth);

        int distance = BattlePathFinder.GetGridDistance(
            CurrentGrid,
            target.CurrentGrid);

        return distance <= attackRange;
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

        SetForwardByDelta(dx, dy);
    }

    private void FaceGrid(BattleGrid grid)
    {
        if (grid == null || CurrentGrid == null)
        {
            return;
        }

        int dx = grid.BoardX - CurrentGrid.BoardX;
        int dy = grid.BoardY - CurrentGrid.BoardY;

        SetForwardByDelta(dx, dy);
    }

    private void SetForwardByDelta(int dx, int dy)
    {
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

    private bool IsEngagedWithTarget()
    {
        if (target == null ||
            target.CurrentGrid == null ||
            CurrentGrid == null)
        {
            return false;
        }

        int distance = BattlePathFinder.GetGridDistance(
            CurrentGrid,
            target.CurrentGrid);

        return distance <= 1;
    }
}


public enum BattleTeam
{
    Player,
    Enemy
}