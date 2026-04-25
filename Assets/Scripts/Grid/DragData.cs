using UnityEngine;

public class DragData : MonoBehaviour
{
    [SerializeField] private NewGridManager gridManager;
    [SerializeField] private UnitManager unitManager;
    [SerializeField] private UnitPresenter unitPresenter;

    private UnitView selectedUnitView;
    private Vector2Int startGridPos;

    public bool IsDragging => selectedUnitView != null;
    public UnitView CurrentView => selectedUnitView;

    private void Update()
    {
        if (Camera.main == null || gridManager == null || unitManager == null || unitPresenter == null)
        {
            return;
        }

        if (Input.GetMouseButtonDown(0))
        {
            TryBeginDrag();
        }

        if (Input.GetMouseButton(0) && selectedUnitView != null)
        {
            DragFollowMouse();
        }

        if (Input.GetMouseButtonUp(0) && selectedUnitView != null)
        {
            DropUnit();
        }
    }

    private void TryBeginDrag()
    {
        Vector2 worldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Collider2D hit = Physics2D.OverlapPoint(worldPos);

        if (hit == null) return;

        UnitView view = hit.GetComponent<UnitView>();
        if (view == null || view.Model == null) return;
        if (view.Team != TeamType.Player) return;

        selectedUnitView = view;
        startGridPos = view.Model.GridPos;
    }

    private void DragFollowMouse()
    {
        Vector2 worldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        selectedUnitView.SetPosition(new Vector3(
            worldPos.x,
            worldPos.y,
            selectedUnitView.transform.position.z
        ));
    }

    private void DropUnit()
    {
        Vector2 worldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2Int dropGridPos = gridManager.WorldToGrid(worldPos);

        bool moved = unitManager.MoveUnit(selectedUnitView.Model, dropGridPos);

        if (moved)
        {
            unitPresenter.SyncUnitPosition(selectedUnitView.Model);
        }
        else
        {
            selectedUnitView.SetPosition(gridManager.GridToWorld(startGridPos));
        }

        selectedUnitView = null;
    }
}
