using System.Collections.Generic;
using UnityEngine;

public class UnitManager : MonoBehaviour
{
    [SerializeField] private NewGridManager gridManager;
    //全てのユニットを管理するリスト
    private List<UnitModel> _units = new List<UnitModel>();

    //座標でユニットを管理する
    private Dictionary<Vector2Int, UnitModel> _unitPositions = new Dictionary<Vector2Int, UnitModel>();

    public IReadOnlyList<UnitModel> Units => _units;


    


    //指定した座標にユニットを配置する
    public bool AddUnit(UnitModel unit, Vector2Int pos)
    {
        if (unit == null) return false;
        if (_unitPositions.ContainsKey(pos)) return false;
        if (HasTile(pos) == false) return false;

        _units.Add(unit);
        _unitPositions[pos] = unit;
        unit.SetGridPos(pos);

        return true;
    }

    //設置したユニットを削除する
    public void RemoveUnit(UnitModel unit)
    {
        if (unit == null) return;

        _units.Remove(unit);

        if (_unitPositions.ContainsKey(unit.GridPos))
        {
            _unitPositions.Remove(unit.GridPos);
        }
    }

    //移動先が使われているか見て移動する
    public bool MoveUnit(UnitModel unit, Vector2Int newPos)
    {
        if (unit == null) return false;
        if (_unitPositions.ContainsKey(newPos)) return false;

        if (_unitPositions.ContainsKey(unit.GridPos))
        {
            _unitPositions.Remove(unit.GridPos);
        }

        unit.SetGridPos(newPos);
        _unitPositions[newPos] = unit;

        return true;
    }

    //指定した座標にユニットがいるか見て返す
    //クリック時にユニットの情報を表示するためなどに
    public UnitModel GetUnitAt(Vector2Int pos)
    {
        _unitPositions.TryGetValue(pos, out UnitModel unit);
        return unit;
    }

    //指定した座標にユニットがいるか見て返す
    public bool IsOccupied(Vector2Int pos)
    {
        return _unitPositions.ContainsKey(pos);
    }

    //指定したチームのユニットを全て返す
    public List<UnitModel> GetUnitsByTeam(UnitModel.TeamType team)
    {
        List<UnitModel> result = new List<UnitModel>();

        foreach (var unit in _units)
        {
            if (unit.Team == team)
            {
                result.Add(unit);
            }
        }

        return result;
    }

    //指定したユニットから最も近い敵ユニットを見つける
    public UnitModel FindNearestEnemy(UnitModel me)
    {
        if (me == null) return null;

        UnitModel nearest = null;
        float minDistance = float.MaxValue;

        foreach (var unit in _units)
        {
            if (unit == me) continue;
            if (unit.Team == me.Team) continue;
            if (unit.CurrentHp <= 0) continue;

            float distance =
                Mathf.Abs(me.GridPos.x - unit.GridPos.x) +
                Mathf.Abs(me.GridPos.y - unit.GridPos.y);
            if (distance < minDistance)
            {
                minDistance = distance;
                nearest = unit;
            }
        }

        return nearest;
    }

    private bool HasTile(Vector2Int pos)
    {
        return gridManager != null && gridManager.GetTileAtPosition(pos) != null;
    }
}

