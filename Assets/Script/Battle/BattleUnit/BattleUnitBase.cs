using System.Collections.Generic;
using UnityEngine;

public class BattleUnitBase : MonoBehaviour
{
    [SerializeField] private BattleUnitView unitView;
    [SerializeField] private DamagePopupManager damagePopupManager;
    [SerializeField] private int chatterLimit = 2;
    [SerializeField] private float chatterStopTime = 0.5f;

    [Header("マナの増加量")]
    [SerializeField] private int normalAttackManaGain = 10;
    [SerializeField] private int takeDamageManaGain = 10;
    [SerializeField] private BattleActionRangeVisualizer rangeVisualizer;

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

            AttackActionData actionData = GetCurrentActionData();

            if (actionData == null)
            {
                return false;
            }

            if (IsEngagedWithTarget() || CanUseAction(actionData))
            {
                return false;
            }

            return true;
        }
    }


    public void Initialize(
        UnitInstance unitInstance,
        BattleTeam teamId,
        DamagePopupManager damagePopupManager)
    {
        UnitInstance = unitInstance;
        Status = unitInstance.Status;
        Team = teamId;
        this.damagePopupManager = damagePopupManager;

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
    /// 自信をグリットに登録する関数
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

        foreach (ItemInstance item in UnitInstance.EquippedItems)
        {
            item?.Data?.OnBattleStart(Status);
        }

        isBattling = true;
        isMoving = false;
        target = null;
        currentPath.Clear();
        InvokeItemBattleStart();
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

    public void ResetAfterBattle(BattleGrid restoreGrid)
    {
        StopBattle();

        target = null;
        previousGrid = null;
        attackTimer = 0f;

        if (Status != null)
        {
            Status.HPReset();
            Status.MPReset();
        }

        if (unitView != null)
        {
            unitView.ResetColor();
        }

        gameObject.SetActive(true);
        SetCurrentGrid(restoreGrid);
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

        AttackActionData actionData = GetCurrentActionData();

        if (actionData == null)
        {
            return;
        }

        if (!target.IsStoppedOnGrid)
        {
            return;
        }

        FaceTarget();

        if (IsEngagedWithTarget() || CanUseAction(actionData))
        {
            currentPath.Clear();
            Attack();
        }
    }

    public BattleGrid GetNextMoveGrid()
    {
        if (!CanRequestMove || UnitInstance == null || UnitInstance.Data == null)
        {
            return null;
        }

        if (target == null)
        {
            return null;
        }

        BattleGrid targetGrid = target.PathTargetGrid;

        if (targetGrid == null)
        {
            return null;
        }

        AttackActionData actionData = GetCurrentActionData();

        if (actionData == null)
        {
            return null;
        }

        FaceTarget();

        if (target.IsStoppedOnGrid &&
            (IsEngagedWithTarget() || CanUseAction(actionData)))
        {
            currentPath.Clear();
            return null;
        }

        BattleGrid nextGrid;

        if (target.IsStoppedOnGrid)
        {
            // 敵が止まっているなら、敵の周囲へ回り込む
            nextGrid = BattlePathFinder.GetNextGridTowardStoppedTarget(
                CurrentGrid,
                targetGrid,
                actionData.CastRange);
        }
        else
        {
            // 敵がまだ動いているなら、最短方向へ前進する
            nextGrid = BattlePathFinder.GetNextGridTowardTarget(
                CurrentGrid,
                targetGrid);
        }

        if (nextGrid == null || nextGrid == targetGrid)
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

    public BattleGrid PathTargetGrid
    {
        get
        {
            if (isMoving && moveTargetGrid != null)
            {
                return moveTargetGrid;
            }

            return CurrentGrid;
        }
    }

    public bool IsStoppedOnGrid
    {
        get
        {
            return !isMoving &&
                   CurrentGrid != null &&
                   CurrentGrid.CurrentBattleUnit == this;
        }
    }

    private void Attack()
    {
        attackTimer -= Time.fixedDeltaTime;

        if (attackTimer > 0)
        {
            return;
        }

        SkillData skill = UnitInstance.Data.Skill;

        if (skill != null &&
            Status.CurrentMp >= skill.ManaCost &&
            CanUseAction(skill))
        {
            Debug.Log(
                $"{UnitInstance.Data.CharacterName} uses skill {skill.SkillName}. " +
                $"MP: {Status.CurrentMp}/{Status.MaxMp}, Cost: {skill.ManaCost}");

            Status.ConsumeAllMana();
            InvokeItemSkillUsed();
            ExecuteAction(skill);
            attackTimer = Status.AttackSpeed;
            return;
        }

        NormalAttackData normalAttack = UnitInstance.Data.NormalAttack;

        if (normalAttack != null && CanUseAction(normalAttack))
        {
            Debug.Log(
                $"{UnitInstance.Data.CharacterName} uses normal attack. " +
                $"MP: {Status.CurrentMp}/{Status.MaxMp}");

            ExecuteAction(normalAttack);
            Status.AddMana(normalAttackManaGain);

            Debug.Log(
                $"{UnitInstance.Data.CharacterName} gained mana by attack. " +
                $"MP: {Status.CurrentMp}/{Status.MaxMp}");

            attackTimer = Status.AttackSpeed;
        }
    }

    public void TakeDamage(float damage)
    {
        if (Status == null || IsDead)
        {
            return;
        }

        int beforeMp = Status.CurrentMp;

        Status.TakeDamage(damage);
        Status.AddMana(takeDamageManaGain);

        if (damagePopupManager != null)
        {
            damagePopupManager.ShowDamage(Mathf.RoundToInt(damage), transform);
        }

        Debug.Log(
            $"{UnitInstance.Data.CharacterName} took {damage} damage. " +
            $"HP: {Status.CurrentHp}/{Status.MaxHp}, " +
            $"MP: {beforeMp} -> {Status.CurrentMp}/{Status.MaxMp}");

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
        InvokeItemOwnerDeath();

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

    public void Heel(float Heal) 
    {
        UnitHeal.HealUnit(Status, Heal);
    }

    private void InvokeItemBattleStart()
    {
        if (UnitInstance == null || UnitInstance.EquippedItems == null)
        {
            return;
        }

        foreach (ItemInstance item in UnitInstance.EquippedItems)
        {
            item?.Data?.OnBattleStart(Status);
        }
    }

    private void InvokeItemNormalAttack(UnitStatus targetStatus)
    {
        if (UnitInstance == null || UnitInstance.EquippedItems == null)
        {
            return;
        }

        foreach (ItemInstance item in UnitInstance.EquippedItems)
        {
            item?.Data?.OnNormalAttack(Status, targetStatus);
        }
    }

    private void InvokeItemSkillUsed()
    {
        if (UnitInstance == null || UnitInstance.EquippedItems == null)
        {
            return;
        }

        foreach (ItemInstance item in UnitInstance.EquippedItems)
        {
            item?.Data?.OnSkillUsed(Status);
        }
    }

    private void InvokeItemOwnerDeath()
    {
        if (UnitInstance == null || UnitInstance.EquippedItems == null)
        {
            return;
        }

        foreach (ItemInstance item in UnitInstance.EquippedItems)
        {
            item?.Data?.OnOwnerDeath(Status);
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

    private bool CanUseAction(AttackActionData actionData)
    {
        if (target == null ||
            target.CurrentGrid == null ||
            CurrentGrid == null ||
            actionData == null)
        {
            return false;
        }

        int range = Mathf.Max(1, actionData.CastRange);

        int distance = BattlePathFinder.GetGridDistance(
            CurrentGrid,
            target.CurrentGrid);

        return distance <= range;
    }

    private void ExecuteAction(AttackActionData actionData)
    {
        List<BattleGrid> targetGrids = GetTargetGrids(actionData);

        if (rangeVisualizer != null)
        {
            rangeVisualizer.FlashRange(actionData, targetGrids);
        }

        List<BattleUnitBase> hitUnits = GetHitUnits(targetGrids);

        SkillEffectContext context = new SkillEffectContext
        {
            Caster = this,
            MainTarget = target,
            ActionData = actionData,
            TargetGrids = targetGrids,
            HitUnits = hitUnits
        };

        SkillData skill = actionData as SkillData;

        if (skill != null && skill.Effects != null && skill.Effects.Length > 0)
        {
            foreach (SkillEffectData effect in skill.Effects)
            {
                if (effect == null)
                {
                    continue;
                }

                effect.Apply(context);
            }

            return;
        }

        ExecuteDamageAction(actionData, hitUnits);
    }

    private void ExecuteDamageAction(
    AttackActionData actionData,
    List<BattleUnitBase> hitUnits)
    {
        int hitCount = Mathf.Max(1, actionData.HitCount);

        for (int i = 0; i < hitCount; i++)
        {
            foreach (BattleUnitBase hitUnit in hitUnits)
            {
                if (hitUnit == null || hitUnit.IsDead)
                {
                    continue;
                }

                DamageResult result = DamageCalculator.CalculateDamage(
                    this,
                    hitUnit,
                    actionData.DamageType,
                    actionData.DamageMultiplier);

                if (!result.IsDodged)
                {
                    hitUnit.TakeDamage(result.Damage);
                    InvokeItemNormalAttack(hitUnit.Status);
                }
            }
        }
    }

    private List<BattleUnitBase> GetHitUnits(List<BattleGrid> targetGrids)
    {
        List<BattleUnitBase> hitUnits = new List<BattleUnitBase>();

        if (targetGrids == null)
        {
            return hitUnits;
        }

        foreach (BattleGrid grid in targetGrids)
        {
            if (grid == null || grid.CurrentBattleUnit == null)
            {
                continue;
            }

            BattleUnitBase hitUnit = grid.CurrentBattleUnit;

            if (hitUnit.Team == Team)
            {
                continue;
            }

            if (!hitUnits.Contains(hitUnit))
            {
                hitUnits.Add(hitUnit);
            }
        }

        return hitUnits;
    }

    private List<BattleGrid> GetTargetGrids(AttackActionData actionData)
    {
        List<BattleGrid> grids = new List<BattleGrid>();

        if (actionData == null || actionData.RangeData == null)
        {
            if (target != null && target.CurrentGrid != null)
            {
                grids.Add(target.CurrentGrid);
            }

            return grids;
        }

        BattleGrid originGrid = GetRangeOriginGrid(actionData);

        if (originGrid == null || actionData.RangeData.Offsets == null)
        {
            return grids;
        }

        foreach (Vector2Int offset in actionData.RangeData.Offsets)
        {
            Vector2Int worldOffset = offset;

            if (actionData.RangeData.Origin == ActionRangeOrigin.FrontOfSelf)
            {
                worldOffset = RotateOffsetByForward(offset);
            }

            int boardX = originGrid.BoardX + worldOffset.x;
            int boardY = originGrid.BoardY + worldOffset.y;

            BattleGrid grid =
                BattleGridManager.Instance.GetGridByBoardPosition(boardX, boardY);

            if (grid != null)
            {
                grids.Add(grid);
            }
        }

        return grids;
    }

    private Vector2Int RotateOffsetByForward(Vector2Int offset)
    {
        if (forwardDirection == Vector2Int.up)
        {
            return offset;
        }

        if (forwardDirection == Vector2Int.down)
        {
            return new Vector2Int(-offset.x, -offset.y);
        }

        if (forwardDirection == Vector2Int.right)
        {
            return new Vector2Int(offset.y, -offset.x);
        }

        if (forwardDirection == Vector2Int.left)
        {
            return new Vector2Int(-offset.y, offset.x);
        }

        return offset;
    }

    private BattleGrid GetRangeOriginGrid(AttackActionData actionData)
    {
        if (actionData == null || actionData.RangeData == null)
        {
            return null;
        }

        switch (actionData.RangeData.Origin)
        {
            case ActionRangeOrigin.Target:
                return target != null ? target.CurrentGrid : null;

            case ActionRangeOrigin.Self:
                return CurrentGrid;

            case ActionRangeOrigin.FrontOfSelf:
                return GetFrontGrid();

            default:
                return null;
        }
    }

    private BattleGrid GetFrontGrid()
    {
        if (CurrentGrid == null || BattleGridManager.Instance == null)
        {
            return null;
        }

        int boardX = CurrentGrid.BoardX + forwardDirection.x;
        int boardY = CurrentGrid.BoardY + forwardDirection.y;

        return BattleGridManager.Instance.GetGridByBoardPosition(boardX, boardY);
    }

    private AttackActionData GetCurrentActionData()
    {
        if (UnitInstance == null || UnitInstance.Data == null || Status == null)
        {
            return null;
        }

        SkillData skill = UnitInstance.Data.Skill;

        if (skill != null && Status.CurrentMp >= skill.ManaCost)
        {
            return skill;
        }

        return UnitInstance.Data.NormalAttack;
    }
}


public enum BattleTeam
{
    Player,
    Enemy
}