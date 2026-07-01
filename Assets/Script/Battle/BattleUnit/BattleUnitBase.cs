using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 戦闘時のデータ
/// </summary>
public class BattleUnitBase : MonoBehaviour
{
    [SerializeField] private Image unitImage;

    public UnitInstance UnitInstance { get; private set; }
    public UnitStatus Status { get; private set; }

    public BattleTeam Team { get; private set; }
    public bool IsDead => Status != null && Status.CurrentHp <= 0;

    private BattleUnitBase target;
    private float attackTimer;
    private bool isBattling;

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

    public void StartBattle()
    {
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
        if (BattleManager.Instance == null)
        {
            return;
        }

        var enemies = BattleManager.Instance.GetEnemies(Team);

        BattleUnitBase nearest = null;
        float nearestDistance = float.MaxValue;

        foreach (BattleUnitBase enemy in enemies)
        {
            if (enemy == null || enemy.IsDead)
            {
                continue;
            }

            float distance = Vector3.Distance(
                transform.position,
                enemy.transform.position);

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

        float distance = Vector3.Distance(
            transform.position,
            target.transform.position);

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
        if (target == null)
        {
            return;
        }

        transform.position = Vector3.MoveTowards(
            transform.position,
            target.transform.position,
            Status.MoveSpeed * Time.fixedDeltaTime);
    }

    private void Attack()
    {
        attackTimer -= Time.fixedDeltaTime;

        if (attackTimer > 0)
        {
            return;
        }

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