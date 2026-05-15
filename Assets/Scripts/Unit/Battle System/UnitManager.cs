using System.Collections.Generic;
using UnityEngine;

public class UnitManager : MonoBehaviour
{
    [SerializeField] private GridManager gridManager;
    //全てのユニットを管理するリスト
    private List<UnitModel> _units = new List<UnitModel>();

    //座標でユニットを管理する
    private Dictionary<Vector2Int, UnitModel> _unitPositions = new Dictionary<Vector2Int, UnitModel>();

    public IReadOnlyList<UnitModel> Units => _units;

    //指定した座標にユニットを配置する
    public bool AddUnit(UnitModel unit, Vector2Int pos)
    {
        if (unit == null) return false;
        //すでにその座標にユニットがいるか見て、いなければ配置する
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

        //ユニットの座標を見てそこに登録されていたら削除
        if (_unitPositions.ContainsKey(unit.GridPos))
        {
            _unitPositions.Remove(unit.GridPos);
        }
    }

   /// <summary>
   /// ユニットを指定した位置に移動させる。
   /// </summary>
   /// <param name="unit">移動させたいユニット</param>
   /// <param name="newPos">移動させたい位置</param>
   /// <returns></returns>
    public bool MoveUnit(UnitModel unit, Vector2Int newPos)
    {
        if (unit == null) return false;
        if (HasTile(newPos) == false) return false;

        if (unit.GridPos == newPos) return true;

        //移動先にすでにユニットがいるか見て、いなければ移動する
        if (_unitPositions.TryGetValue(newPos, out UnitModel otherUnit) && otherUnit != unit)
        {
            return false;
        }

        //前の移動先の登録を削除
        _unitPositions.Remove(unit.GridPos);

        unit.SetGridPos(newPos);
        _unitPositions[newPos] = unit;

        return true;
    }

    //指定した座標にユニットがいるか見て返す
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
    public List<UnitModel> GetUnitsByTeam(TeamType team)
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

    /// <summary>
    /// 指定した位置にタイルが存在するか確認する
    /// </summary>
    /// <param name="pos">移動する移動先</param>
    /// <returns></returns>
    private bool HasTile(Vector2Int pos)
    {
        return gridManager != null && gridManager.GetTileAtPosition(pos) != null;
    }
}

