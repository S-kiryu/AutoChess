using System;

[Serializable]
public class ItemInstance
{
    private readonly string uniqueId;
    private readonly ItemData data;

    public string UniqueId => uniqueId;
    public ItemData Data => data;

    public ItemInstance(ItemData data)
    {
        uniqueId = Guid.NewGuid().ToString();
        this.data = data;
    }
}