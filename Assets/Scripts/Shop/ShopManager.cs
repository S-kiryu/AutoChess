using UnityEngine;
using UnityEngine.UI;

public class ShopManager : MonoBehaviour
{
    [SerializeField] private ShopData _shopData;
    [SerializeField] private ShopView _shopView;
    [SerializeField] private Button _rollButton;

    private ShopPresenter _shopPresenter;

    private void Start()
    {
        var shopModel = new ShopModel(_shopData);
        _shopPresenter = new ShopPresenter(shopModel, _shopView);

        _rollButton.onClick.AddListener(OnClickRoll);
    }

    private void OnClickRoll()
    {
        Debug.Log("ƒNƒŠƒbƒN‚³‚ê‚½");
        int playerLevel = 1;
        _shopPresenter.Roll(playerLevel);
    }
}
