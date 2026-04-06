using UnityEngine;

[System.Serializable]
public class ShopLevelData
{
    [Header("コスト1〜7の確率")]
    public int[] costRates = new int[7];
}