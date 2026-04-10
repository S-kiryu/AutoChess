using Unity.VisualScripting;
using UnityEngine;

public class ShopModel
{
    private ShopData _shopData;

    public ShopModel(ShopData shopData)
    {
        _shopData = shopData;
    }

    // レベルに応じたユニットをランダムに取得
    public UnitData GetRandomUnit()
    {
        return _shopData.GetRandom();
    }
}
