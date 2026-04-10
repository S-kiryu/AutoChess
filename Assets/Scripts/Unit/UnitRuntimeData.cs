using UnityEngine;

public class UnitRuntimeData : MonoBehaviour
{
    public Vector2Int GridPos { get; private set; }

    //ユニットの移動先を更新する
    public void SetGridPos(Vector2Int newGridPos)
    {
        GridPos = newGridPos;
    }
}
