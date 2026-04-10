using UnityEngine;

[CreateAssetMenu(fileName = "ShopData", menuName = "ScriptableObjects/ShopData")]
public class ShopData : ScriptableObject
{
    public UnitData[] Units = new UnitData[5];
    public int[] LevelUpCost;

    public UnitData GetRandom()
    {
        int index = Random.Range(0, Units.Length);
        return Units[index];
    }
}