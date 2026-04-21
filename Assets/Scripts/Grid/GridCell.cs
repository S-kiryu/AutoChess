using UnityEngine;

public class GridCell
{
    public Vector2Int pos;
    public UnitModel unit;

    //ƒ†ƒjƒbƒg‚ª‚¢‚é‚©‚Ç‚¤‚©
    public bool IsEmpty => unit == null;
}
