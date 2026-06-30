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


    private void FixedUpdate()
    {
        if (Status == null || IsDead)
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

    public void Initialize(UnitInstance unitInstance, BattleTeam teamId)
    {
        UnitInstance = unitInstance;
        Status = unitInstance.Status;
        Team = teamId;

        attackTimer = Status.AttackSpeed;

        UpdateView();
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

    //一番近い敵を見つけてそこに動く
    private void FindNearestEnemy()
    {
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

    //攻撃が届くかどうかを確認して行動を決める
    public void AttackRangeCheck()
    {
        if (target == null)
        {
            return;
        }

        float distance = Vector3.Distance(transform.position, target.transform.position);

        if (distance <= Status.AttackRange)
        {
            Attack();
        }
        else
        {
            FindNearestEnemy();
        }
    }

    private void Attack()
    {
        attackTimer -= Time.deltaTime;

        if (attackTimer > 0)
        {
            return;
        }

        target.TakeDamage(Status.Attack);
        attackTimer = Status.AttackSpeed;
    }

    public void TakeDamage(float damage)
    {
        Status.TakeDamage(damage);

        if (Status.IsDead)
        {
            Die();
        }
    }

    private void Die()
    {
        gameObject.SetActive(false);
    }
}
public enum BattleTeam
{
    Player,
    Enemy
}