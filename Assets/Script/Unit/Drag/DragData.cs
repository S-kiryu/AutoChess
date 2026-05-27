//ドラック中のデータを保持するクラス
public static class DragData
{
    public static UnitInstance Unit { get; private set; }

    public static void SetUnit(UnitInstance unit)
    {
        Unit = unit;
    }

    public static void Clear()
    {
        Unit = null;
    }
}