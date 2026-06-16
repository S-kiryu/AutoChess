using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;

public class BattleGrid : MonoBehaviour, IDropHandler
{
    //今いるユニット
    private UnitInstance _currentUnit;
    //グリットの座標
    private Vector2Int _position;

    public UnitInstance CurrentUnit => _currentUnit;
    public Vector2Int Position => _position;

    public void OnDrop(PointerEventData eventData)
    {
        //戦闘中だったら何もできないようにする

    }
    public void Initialize(Vector2Int pos)
    {
        _position = pos;
    }

    public void SetUnit(UnitInstance unit) 
    {
        _currentUnit = unit;
    }
}
