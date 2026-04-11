using UnityEngine;

public class GridCell : MonoBehaviour
{
    public Vector2Int pos;
    public Unit unit;

    //ƒ†ƒjƒbƒg‚ª‚¢‚é‚©‚Ç‚¤‚©
    public bool IsEmpty => unit == null;
}
