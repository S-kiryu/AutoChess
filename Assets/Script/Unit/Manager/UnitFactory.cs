public static class UnitFactory
{
    /// <summary>
    /// UnitInstanceを生成するためのファクトリークラス
    /// </summary>
    /// <param name="data"></param>
    public static UnitInstance Create(CharacterData data)
    {
        var unit = new UnitInstance();
        unit.Initialize(data);
        return unit;
    }
}