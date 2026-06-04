using UnityEngine;
using UnityEngine.UI;

public class ShopUI : MonoBehaviour
{
    [SerializeField] Button _button;
    [SerializeField] ShopManager shopManager;
    [SerializeField] int _generateCount = 1;

    public void OnClickGenerate() 
    {
        shopManager.GenerateUnit(_generateCount);
    }
}
