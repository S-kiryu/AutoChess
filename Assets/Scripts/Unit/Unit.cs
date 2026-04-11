using UnityEngine;
using static UnitModel;

public class Unit : MonoBehaviour
{
    public Vector2Int GridPos { get; private set; }
    public UnitData Data { get; private set; }
    public BaseStatus RuntimeStatus { get; private set; }
    private float _attackCooldown;
    private TeamType Team;
    //ユニットの初期化
    public void Initialize(UnitData data, Vector2Int initialGridPos)
    {
        Data = data;
        RuntimeStatus = data.baseStatus;
        GridPos = initialGridPos;
    }

    //ユニットの行動処理
    public void Tick(GridManager grid)
    {
        _attackCooldown -= Time.deltaTime;

        // ターゲットを見つける
        var target = FindTarget(grid);
        if (target == null) return;

        // ターゲットとの距離を計算
        float distance = Vector2Int.Distance(GridPos, target.GridPos);

        // 攻撃範囲内なら攻撃、そうでなければ移動
        if (distance <= RuntimeStatus.AttackRange)
        {
            if (_attackCooldown <= 0f)
            {
                TryAttack(target);

                // 攻撃速度からクールタイムを計算
                _attackCooldown = 1f / RuntimeStatus.AttackSpeed;
            }
        }
        else
        {
            MoveTowards(target, grid);
        }
    }

    //ユニットの移動先を更新する
    public void SetGridPos(Vector2Int newGridPos)
    {
        GridPos = newGridPos;
    }

    public void TakeDamage(int damage)
    {
        Data.baseStatus.Hp -= damage;
        if (Data.baseStatus.Hp <= 0)
        {
            Die();
        }
    }

    public void Die()
    {
        //死亡処理
    }

    private Unit FindTarget(GridManager grid)
    {
        Unit nearest = null;
        float minDist = float.MaxValue;

        foreach (var cell in grid.AllCells)
        {
            if (cell.unit == null) continue;
            if (cell.unit == this) continue;
            if (cell.unit.Team == this.Team) continue;

            float dist = Vector2Int.Distance(GridPos, cell.unit.GridPos);

            if (dist < minDist)
            {
                minDist = dist;
                nearest = cell.unit;    
            }
        }

        return nearest;
    }

    private void TryAttack(Unit target)
    {
        if (_attackCooldown > 0) return;

        target.TakeDamage((int)RuntimeStatus.Attack);
    }

    private void MoveTowards(Unit target, GridManager board)
    {
        Vector2Int dir = target.GridPos - GridPos;

        Vector2Int move = new Vector2Int(
            Mathf.Clamp(dir.x, -1, 1),
            Mathf.Clamp(dir.y, -1, 1)
        );

        Vector2Int nextPos = GridPos + move;

        if (board.CanPlace(nextPos))
        {
            board.MoveUnit(this, nextPos);
        }
    }
}
