using System;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;

public class MoneyManager : MonoBehaviour
{
    public static MoneyManager instance {get;private set; }

    [SerializeField]
    private half money;

    [SerializeField]
    private half UnitPrice;
    [SerializeField] private ShopFactory shopFactory;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
    }

    public void GetMoney(int addMoney) 
    {
        money += (half)addMoney;
    }

    public void BuyUnit() 
    {
        if(money >= UnitPrice)
        {
            money -= UnitPrice;
            shopFactory.OnClickGenerate();
            Debug.Log($"ユニットを購入しました。残りの所持金: {money}");
        }
        else
        {
            Debug.Log("所持金が不足しています。");
        }
    }
}
