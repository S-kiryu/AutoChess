using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopUI : MonoBehaviour
{
    [SerializeField] Button _button;
    [SerializeField] ShopManager _shopManager;

    public void OnClickGenerate() 
    {
        var Units = _shopManager.GenerateShop();
    }
}
