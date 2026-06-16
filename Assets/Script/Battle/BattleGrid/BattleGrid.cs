using UnityEngine;

/// <summary>
/// 戦闘グリッドの1マス
/// </summary>
public class BattleGridCell : MonoBehaviour
{
    private bool _isUnitPresent;
    private Vector2Int _position;

    public Vector2Int Position => _position;
    public bool IsUnitPresent => _isUnitPresent;

    public void Initialize(Vector2Int position)
    {
        _position = position;
        _isUnitPresent = false;
    }

    public void SetUnitPresent(bool isPresent)
    {
        _isUnitPresent = isPresent;
    }
}