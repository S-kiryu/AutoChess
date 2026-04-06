using UnityEngine;

[CreateAssetMenu(fileName = "ShopData", menuName = "ScriptableObjects/ShopData")]
public class ShopData : ScriptableObject
{
    [Header("コストごとのユニット")]
    [SerializeField] private CostGroup[] unitsByCost = new CostGroup[7];
    [Header("レベルごとの排出率")]
    public ShopLevelData[] levelData = new ShopLevelData[10];
}