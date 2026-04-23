using System.Collections.Generic;
using UnityEngine;

public class UnitPresenter : MonoBehaviour
{
    [SerializeField] private UnitManager unitManager;

    private Dictionary<UnitModel, UnitView> _views = new Dictionary<UnitModel, UnitView>();

    /// <summary>
    /// ユニットを登録する。モデルとビューを紐づけて管理する。
    /// </summary>
    public void RegisterUnit(UnitModel model, UnitView view, Vector2Int gridPos)
    {
        if (model == null || view == null) return;

        unitManager.AddUnit(model, gridPos);
        _views[model] = view;

        view.SetPosition(GridToWorld(gridPos));
        view.SetHP(model.CurrentHp);
    }

    // ユニットの位置をモデルからビューに同期する
    public void SyncUnitPosition(UnitModel model)
    {
        if (model == null) return;
        if (_views.TryGetValue(model, out UnitView view) == false) return;

        view.SetPosition(GridToWorld(model.GridPos));
    }

    // ユニットのHPをモデルからビューに同期する
    public void SyncUnitHP(UnitModel model)
    {
        if (model == null) return;
        if (_views.TryGetValue(model, out UnitView view) == false) return;

        view.SetHP(model.CurrentHp);
    }

    // ユニットを削除する
    public void RemoveUnit(UnitModel model)
    {
        if (model == null) return;
        if (_views.TryGetValue(model, out UnitView view))
        {
            Destroy(view.gameObject);
            _views.Remove(model);
        }

        unitManager.RemoveUnit(model);
    }

    //ユニットをグリッド座標からワールド座標に変換する
    private Vector3 GridToWorld(Vector2Int gridPos)
    {
        return new Vector3(gridPos.x, gridPos.y,0f );
    }
}
