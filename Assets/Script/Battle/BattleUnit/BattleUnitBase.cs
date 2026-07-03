using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 戦闘時のデータ
/// </summary>
public class BattleUnitBase : MonoBehaviour
{
    [SerializeField] private Image unitImage;

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

    public void Initialize(UnitInstance unitInstance, BattleTeam teamId)
    {
        UnitInstance = unitInstance;
        Status = unitInstance.Status;
        Team = teamId;

        attackTimer = Status.AttackSpeed;

        UpdateView();

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

    private void UpdateView()
    {
        if (unitImage == null || UnitInstance == null || UnitInstance.Data == null)
        {
            return;
        }

        unitImage.sprite = UnitInstance.Data.Icon;
        unitImage.enabled = true;
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

            int distance = BattleGridManager.Instance.GetGridDistance(
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
        if (target == null || target.CurrentGrid == null || CurrentGrid == null)
        {
            return;
        }

        int distance = BattleGridManager.Instance.GetGridDistance(
            CurrentGrid,
            target.CurrentGrid);

        if (distance <= Status.AttackRange)
        {
            Attack();
        }
        else
        {
            MoveToTarget();
        }
    }

    private void MoveToTarget()
    {
        if (target == null || target.CurrentGrid == null || CurrentGrid == null)
        {
            return;
        }

        BattleGrid nextGrid = BattleGridManager.Instance.GetNextGridToward(
            CurrentGrid,
            target.CurrentGrid);

        if (nextGrid == null)
        {
            return;
        }

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

    private void Attack()
    {
        attackTimer -= Time.fixedDeltaTime;

        if (attackTimer > 0)
        {
            return;
        }

        Debug.Log($"{name} が {target.name} を攻撃");

        target.TakeDamage(Status.Attack);
        attackTimer = Status.AttackSpeed;
    }

    public void TakeDamage(float damage)
    {
        if (Status == null || IsDead)
        {
            return;
        }

        Status.TakeDamage(damage);

        if (Status.IsDead)
        {
            Die();
        }
    }

    private void Die()
    {
        isBattling = false;
        isMoving = false;

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
}

public enum BattleTeam
{
    Player,
    Enemy
}