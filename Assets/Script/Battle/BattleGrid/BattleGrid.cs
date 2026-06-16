using UnityEngine;

public class BattleGrid : MonoBehaviour
{
    private bool _isUnitPresent;
    private Vector2Int _position;

    public bool IsUnitPresent() 
    {
        return _isUnitPresent;
    }

    public void SetPos(Vector2Int pos) 
    {
        _position = pos;
    }
}
