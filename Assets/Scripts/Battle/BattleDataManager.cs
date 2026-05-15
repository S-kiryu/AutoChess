using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class BattleDataManager : MonoBehaviour
{
   public static BattleDataManager Instance { get; private set; }
    //編成中のユニットデータを保持するリスト
    public List<UnitData> PlayerUnits = new List<UnitData>();


    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {

    }
}
