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
    public UnitData GetRandomUnit(int level)
    {
        var rates = _shopData._levelData[level-1].costRates;

        int cost = GetRandomCost(rates);

        return _shopData._costGroup[cost - 1].GetRandom();
    }


    // コスト1〜7の確率からランダムにコストを決定
    private int GetRandomCost(int[] rates)
    {
        int total = 0;
        foreach (var r in rates) total += r;

        int rand = Random.Range(0, total);

        for (int i = 0; i < rates.Length; i++)
        {
            rand -= rates[i];
            if (rand < 0)
                return i + 1;
        }

        return 1;
    }
}
