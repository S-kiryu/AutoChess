using UnityEngine;

public class BattleUnitMovement
{
    private readonly BattleUnitBase owner;
    private readonly int chatterLimit;
    private readonly float chatterStopTime;

    private BattleGrid moveTargetGrid;
    private BattleGrid previousGrid;
    private int chatterCount;
    private float moveStopTimer;
    private bool isMoving;
    private Vector3 moveDestination;
    private Vector2Int forwardDirection = Vector2Int.up;

    public BattleUnitMovement(
        BattleUnitBase owner,
        int chatterLimit,
        float chatterStopTime)
    {
        this.owner = owner;
        this.chatterLimit = chatterLimit;
        this.chatterStopTime = chatterStopTime;
    }

    public bool IsMoving => isMoving;
    public Vector2Int ForwardDirection => forwardDirection;

    public BattleGrid PathTargetGrid
    {
        get
        {
            if (isMoving && moveTargetGrid != null)
            {
                return moveTargetGrid;
            }

            return owner.CurrentGrid;
        }
    }

    public bool CanRequestMove
    {
        get
        {
            if (moveStopTimer > 0f ||
                isMoving ||
                owner.Status == null ||
                owner.IsDead ||
                owner.Target == null ||
                owner.Target.CurrentGrid == null ||
                owner.CurrentGrid == null)
            {
                return false;
            }

            AttackActionData actionData = owner.GetCurrentActionData();

            if (actionData == null)
            {
                return false;
            }

            return !owner.IsEngagedWithTarget() &&
                   !owner.CanUseAction(actionData);
        }
    }

    public void TickMoveStopTimer(float deltaTime)
    {
        if (moveStopTimer > 0f)
        {
            moveStopTimer -= deltaTime;
        }
    }

    public void ClearMoveLock()
    {
        if (moveTargetGrid != null)
        {
            moveTargetGrid.ClearMoveLock(owner);
            moveTargetGrid = null;
        }
    }

    public void ResetState()
    {
        ClearMoveLock();

        previousGrid = null;
        chatterCount = 0;
        moveStopTimer = 0f;
        isMoving = false;
    }

    public BattleGrid GetNextMoveGrid()
    {
        if (!CanRequestMove ||
            owner.UnitInstance == null ||
            owner.UnitInstance.Data == null)
        {
            return null;
        }

        BattleUnitBase target = owner.Target;

        if (target == null)
        {
            return null;
        }

        BattleGrid targetGrid = target.PathTargetGrid;

        if (targetGrid == null)
        {
            return null;
        }

        AttackActionData actionData = owner.GetCurrentActionData();

        if (actionData == null)
        {
            return null;
        }

        FaceTarget();

        if (target.IsStoppedOnGrid &&
            (owner.IsEngagedWithTarget() ||
             owner.CanUseAction(actionData)))
        {
            return null;
        }

        BattleGrid nextGrid;

        if (target.IsStoppedOnGrid)
        {
            nextGrid = BattlePathFinder.GetNextGridTowardStoppedTarget(
                owner.CurrentGrid,
                targetGrid,
                actionData.CastRange);
        }
        else
        {
            nextGrid = BattlePathFinder.GetNextGridTowardTarget(
                owner.CurrentGrid,
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
                return;
            }
        }
        else
        {
            chatterCount = 0;
        }

        FaceGrid(nextGrid);
        owner.PlayMoveAnimation(forwardDirection);

        moveTargetGrid = nextGrid;
        moveDestination = moveTargetGrid.transform.position;
        isMoving = true;
    }

    public void ContinueMove()
    {
        owner.transform.position = Vector3.MoveTowards(
            owner.transform.position,
            moveDestination,
            owner.Status.MoveSpeed * Time.fixedDeltaTime);

        if (Vector3.Distance(owner.transform.position, moveDestination) > 0.01f)
        {
            return;
        }

        if (moveTargetGrid == null)
        {
            isMoving = false;
            owner.StopMoveAnimation();
            return;
        }

        owner.transform.position = moveTargetGrid.transform.position;

        BattleGrid oldGrid = owner.CurrentGrid;

        if (owner.CurrentGrid != null)
        {
            owner.CurrentGrid.ClearBattleUnit(owner);
        }

        moveTargetGrid.ClearMoveLock(owner);

        owner.SetCurrentGridFromMovement(moveTargetGrid);
        previousGrid = oldGrid;

        moveTargetGrid = null;
        isMoving = false;
    }

    public void FaceTarget()
    {
        BattleUnitBase target = owner.Target;

        if (target == null ||
            target.CurrentGrid == null ||
            owner.CurrentGrid == null)
        {
            return;
        }

        int dx = target.CurrentGrid.BoardX - owner.CurrentGrid.BoardX;
        int dy = target.CurrentGrid.BoardY - owner.CurrentGrid.BoardY;

        SetForwardByDelta(dx, dy);
    }

    private void FaceGrid(BattleGrid grid)
    {
        if (grid == null || owner.CurrentGrid == null)
        {
            return;
        }

        int dx = grid.BoardX - owner.CurrentGrid.BoardX;
        int dy = grid.BoardY - owner.CurrentGrid.BoardY;

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
}