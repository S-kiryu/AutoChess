using UnityEngine;

/// <summary>
/// 戦闘時のデータ
/// </summary>
public class BattleUnitBase : MonoBehaviour
{
    public UnitInstance UnitInstance { get; private set; }
    public UnitStatus Status { get; private set; }

    public int TeamId { get; private set; }
    public bool IsDead => Status.CurrentHp <= 0;

    private BattleUnitBase target;
    private float attackTimer;

    private void Start()
    {
        attackTimer = Status.AttackSpeed;
    }

    public void Initialize(UnitInstance unitInstance, int teamId)
    {
        UnitInstance = unitInstance;
        Status = unitInstance.Status;
        TeamId = teamId;
    }

    private void FixedUpdate()
    {
        attackTimer -= Time.deltaTime;

        if (target != null && attackTimer <= 0)
        {
            TakeDamage(Status.Attack);
            attackTimer = Status.AttackSpeed;
        }
    }

    //一番近い敵を見つけてそこに動く
    public void FindNearestEnemy() 
    {
        Vector3 dir = (target.transform.position - transform.position).normalized;
        transform.position += dir * Status.MoveSpeed * Time.deltaTime;
    }

    //攻撃が届くかどうかを確認して行動を決める
    public void AttackRangeCheck() 
    {
        float distance = Vector3.Distance(transform.position, target.transform.position);

        if (distance <= Status.AttackRange)
        {
            //敵に攻撃する行動を書く
        }
        else
        {
            FindNearestEnemy();
        }
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