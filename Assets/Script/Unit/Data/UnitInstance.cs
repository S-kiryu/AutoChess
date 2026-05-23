[System.Serializable]
public class UnitInstance   //キャラの実態
{
    public string UniqueId;          // 個体ID
    public CharacterData Data;       // ベース情報
    private UnitStatus Status;        // ランタイムステータス

    public UnitInstance(CharacterData data)
    {
        UniqueId = System.Guid.NewGuid().ToString();
        Data = data;

        Status = new UnitStatus();
        Status.Initialize(data.BaseStatus);
    }
}
