using System.Collections.Generic;
using UnityEngine;

public class UnitPresenter : MonoBehaviour
{
    public static UnitPresenter Instance { get; private set; }

    [SerializeField] private UnitManager unitManager;

    private readonly Dictionary<UnitModel, UnitView> views = new Dictionary<UnitModel, UnitView>();

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    /// <summary>
    /// 指定したところにユニットを登録する。成功したらtrue、失敗したらfalseを返す。
    /// </summary>
    /// <param name="model">ユニットのデータ</param>
    /// <param name="view">表記させるUI</param>
    /// <param name="gridPos">置く場所</param>
    /// <returns>おけるかどうかのブール値が返って来る</returns>
    public bool RegisterUnit(UnitModel model, UnitView view, Vector2Int gridPos)
    {
        if (model == null || view == null || unitManager == null)
        {
            return false;
        }

        bool added = unitManager.AddUnit(model, gridPos);
        if (!added)
        {
            return false;
        }

        views[model] = view;
        view.Bind(model);
        view.SetPosition(GridToWorld(gridPos));
        view.SetHP(model.CurrentHp);
        return true;
    }


    public void SyncUnitPosition(UnitModel model)
    {
        if (model == null) return;
        if (!views.TryGetValue(model, out UnitView view)) return;

        view.SetPosition(GridToWorld(model.GridPos));
    }

    public void SyncUnitHP(UnitModel model)
    {
        if (model == null) return;
        if (!views.TryGetValue(model, out UnitView view)) return;

        view.SetHP(model.CurrentHp);
    }

    public void RemoveUnit(UnitModel model)
    {
        if (model == null) return;

        if (views.TryGetValue(model, out UnitView view))
        {
            Destroy(view.gameObject);
            views.Remove(model);
        }

        unitManager.RemoveUnit(model);
    }

    private Vector3 GridToWorld(Vector2Int gridPos)
    {
        return new Vector3(gridPos.x, gridPos.y, 0f);
    }
}
