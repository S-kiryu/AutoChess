using UnityEngine;
using UnityEngine.EventSystems;

public class BenchSlot : MonoBehaviour, IDropHandler
{
    private int x;
    private int y;
    private BenchManager benchManager;

    public int X => x;
    public int Y => y;

    public void Initialize(int x, int y, BenchManager benchManager)
    {
        this.x = x;
        this.y = y;
        this.benchManager = benchManager;
    }

    public void OnDrop(PointerEventData eventData)
    {
        // 後でここにドラッグ中ユニットの受け取りを書く
        // 例:
        // UnitInstance unit = ...
        // benchManager.SetUnit(unit, x, y);
    }
}