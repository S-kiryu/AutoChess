using UnityEngine;

[CreateAssetMenu(fileName = "ShopData", menuName = "ScriptableObjects/ShopData")]
public class ShopData : ScriptableObject
{
    [SerializeField] private CostGroup[] unitsByCost = new CostGroup[7];
}