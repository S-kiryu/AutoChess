using System;
using System.Collections.Generic;
using UnityEngine;

public class BattleUnitBase : MonoBehaviour
{
    [SerializeField] private BattleUnitView unitView;
    [SerializeField] private DamagePopupManager damagePopupManager;
    [SerializeField] private BattleActionRangeVisualizer rangeVisualizer;

    [Header("Mana")]
    [SerializeField] private int normalAttackManaGain = 10;
    [SerializeField] private int takeDamageManaGain = 10;

    [Header("Movement")]
    [SerializeField] private int chatterLimit = 2;
    [SerializeField] private float chatterStopTime = 0.5f;

    public UnitInstance UnitInstance { get; private set; }
    public BattleGrid CurrentGrid { get; private set; }
    public UnitStatus Status { get; private set; }
    public BattleTeam Team { get; private set; }

    public BattleUnitBase Target { get; private set; }

    public bool IsDead => Status != null && Status.CurrentHp <= 0;
    public bool IsMoving => movement != null && movement.IsMoving;
    public bool CanRequestMove => movement != null && movement.CanRequestMove;
    public bool IsBattling => isBattling;

    public BattleGrid PathTargetGrid => movement != null
        ? movement.PathTargetGrid
        : CurrentGrid;

    public bool IsStoppedOnGrid =>
        !IsMoving &&
        CurrentGrid != null &&
        CurrentGrid.CurrentBattleUnit == this;

    public Vector2Int ForwardDirection => movement != null
        ? movement.ForwardDirection
        : Vector2Int.up;

    public IEnumerable<ItemInstance> EquippedItems => UnitInstance != null
        ? UnitInstance.EquippedItems
        : Array.Empty<ItemInstance>();

    private bool isBattling;

    private BattleUnitItemHandler itemHandler;
    private BattleUnitTargeting targeting;
    private BattleUnitRangeResolver rangeResolver;
    private BattleUnitAttack attack;
    private BattleUnitDamageReceiver damageReceiver;
    private BattleUnitMovement movement;

    public void Initialize(
        UnitInstance unitInstance,
        BattleTeam teamId,
        DamagePopupManager damagePopupManager)
    {
        UnitInstance = unitInstance;
        Status = unitInstance.Status;
        Team = teamId;
        this.damagePopupManager = damagePopupManager;

        if (unitView == null)
        {
            unitView = GetComponent<BattleUnitView>();
        }

        if (unitView != null)
        {
            unitView.SetUnit(UnitInstance);
        }

        itemHandler = new BattleUnitItemHandler(this);
        targeting = new BattleUnitTargeting(this);
        rangeResolver = new BattleUnitRangeResolver(this);

        movement = new BattleUnitMovement(
            this,
            chatterLimit,
            chatterStopTime);

        attack = new BattleUnitAttack(
            this,
            rangeVisualizer,
            rangeResolver,
            itemHandler,
            normalAttackManaGain);

        damageReceiver = new BattleUnitDamageReceiver(
            this,
            damagePopupManager,
            unitView,
            itemHandler,
            takeDamageManaGain);

        attack.ResetTimer();

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

        SetCurrentGridFromMovement(grid);
    }

    public void SetCurrentGridFromMovement(BattleGrid grid)
    {
        CurrentGrid = grid;

        if (CurrentGrid != null)
        {
            CurrentGrid.SetBattleUnit(this);
            transform.position = CurrentGrid.transform.position;
        }
    }

    public void StartBattle()
    {
        movement.ClearMoveLock();

        isBattling = true;
        Target = null;

        attack.ResetTimer();
        itemHandler.OnBattleStart();
    }

    public void StopBattle()
    {
        isBattling = false;
        Target = null;

        movement.ResetState();
        itemHandler.OnEndBattle();
    }

    public void ResetAfterBattle(BattleGrid restoreGrid)
    {
        StopBattle();

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
        if (movement != null)
        {
            movement.TickMoveStopTimer(Time.fixedDeltaTime);
        }

        if (!isBattling || Status == null || IsDead)
        {
            return;
        }

        if (movement.IsMoving)
        {
            movement.ContinueMove();
            return;
        }

        if (Target == null || Target.IsDead)
        {
            Target = targeting.FindNearestEnemy();
        }

        if (Target == null)
        {
            return;
        }

        AttackRangeCheck();
    }

    public void AttackRangeCheck()
    {
        if (Target == null || UnitInstance == null || UnitInstance.Data == null)
        {
            return;
        }

        AttackActionData actionData = GetCurrentActionData();

        if (actionData == null || !Target.IsStoppedOnGrid)
        {
            return;
        }

        movement.FaceTarget();

        if (IsEngagedWithTarget() || CanUseAction(actionData))
        {
            attack.Tick();
        }
    }

    public BattleGrid GetNextMoveGrid()
    {
        return movement.GetNextMoveGrid();
    }

    public void BeginMoveTo(BattleGrid nextGrid)
    {
        movement.BeginMoveTo(nextGrid);
    }

    public void TakeDamage(float damage)
    {
        damageReceiver.TakeDamage(damage);
    }

    public void DieFromDamage()
    {
        Die();
    }

    private void Die()
    {
        isBattling = false;
        Target = null;

        movement.ResetState();

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

    public void Heel(float heal)
    {
        UnitHeal.HealUnit(Status, heal);
    }

    public bool IsEngagedWithTarget()
    {
        if (Target == null ||
            Target.CurrentGrid == null ||
            CurrentGrid == null)
        {
            return false;
        }

        int distance = BattlePathFinder.GetGridDistance(
            CurrentGrid,
            Target.CurrentGrid);

        return distance <= 1;
    }

    public bool CanUseAction(AttackActionData actionData)
    {
        if (Target == null ||
            Target.CurrentGrid == null ||
            CurrentGrid == null ||
            actionData == null)
        {
            return false;
        }

        int range = Mathf.Max(1, actionData.CastRange);

        int distance = BattlePathFinder.GetGridDistance(
            CurrentGrid,
            Target.CurrentGrid);

        return distance <= range;
    }

    public AttackActionData GetCurrentActionData()
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