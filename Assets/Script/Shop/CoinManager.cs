using UnityEngine;

public class CoinManager : MonoBehaviour
{
    public static CoinManager Instanse { get; private set; }

    [SerializeField]private int Coin;

    private void Awake()
    {
        if (Instanse != null && Instanse != this)
        {
            Destroy(gameObject);
            return;
        }
        Instanse = this;
    }

    public int CurrentCoin => Coin;

    public bool CanPay(int amount)
    {
        return Coin >= amount;
    }

    public bool TryPay(int amount)
    {
        if (amount < 0)
        {
            return false;
        }

        if (Coin < amount)
        {
            Debug.Log($"コインが足りません。必要: {amount}, 所持: {Coin}");
            return false;
        }

        Coin -= amount;
        Debug.Log($"コインを {amount} 支払いました。残り: {Coin}");
        return true;
    }

    public void AddCoin(int amount)
    {
        Coin += amount;
        Debug.Log($"コインを {amount} 追加しました。現在のコイン: {Coin}");
    }

    public void RemoveCoin(int amount) 
    {
        Coin -= amount;
        Debug.Log($"コインを {amount} 減らしました。現在のコイン: {Coin}");
    }
}
