using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// 座標と今いるユニットを持ってるよ
/// </summary>
public class BattleGrid : MonoBehaviour, IDropHandler
{
    //今いるユニット
    private UnitInstance _currentUnit;
    //グリットの座標
    private Vector2Int _position;
    private BattleGridManager _manager;

    public UnitInstance CurrentUnit => _currentUnit;
    public Vector2Int Position => _position;

    public void OnDrop(PointerEventData eventData)
    {
        //戦闘中だったら何もできないようにする

    }
    public void Initialize(Vector2Int pos,BattleGridManager manager)
    {
        _position = pos;
        _manager = manager;
    }

    public void SetUnit(UnitInstance unit) 
    {
        _currentUnit = unit;
    }
}
