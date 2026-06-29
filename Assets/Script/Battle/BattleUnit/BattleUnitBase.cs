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

    public int TeamId { get; private set; }
    public bool IsDead => Status != null && Status.CurrentHp <= 0;

    private BattleUnitBase target;
    private float attackTimer;

    public void Initialize(UnitInstance unitInstance, int teamId)
    {
        UnitInstance = unitInstance;
        Status = unitInstance.Status;
        TeamId = teamId;

        attackTimer = Status.AttackSpeed;

        UpdateView();
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

    public void SetTarget(BattleUnitBase newTarget)
    {
        target = newTarget;
    }

    private void FixedUpdate()
    {
        if (Status == null || IsDead || target == null || target.IsDead)
        {
            return;
        }

        AttackRangeCheck();
    }

    //一番近い敵を見つけてそこに動く
    public void FindNearestEnemy()
    {
        if (target == null)
        {
            return;
        }

        Vector3 dir = (target.transform.position - transform.position).normalized;
        transform.position += dir * Status.MoveSpeed * Time.deltaTime;
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