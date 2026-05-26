public static class UnitFactory
{
    //UnitInstanceを生成するためのファクトリークラス
    public static UnitInstance Create(CharacterData data)
    {
        var unit = new UnitInstance();
        unit.Initialize(data);
        return unit;
    }
}