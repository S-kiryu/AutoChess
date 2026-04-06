using Unity.VisualScripting.Dependencies.Sqlite;
using UnityEngine;

[System.Serializable]
public class CostGroup
{
    public UnitData[] Units;

    public UnitData GetRandom()
    {
        int index = Random.Range(0, Units.Length);
        return Units[index];
    }
}