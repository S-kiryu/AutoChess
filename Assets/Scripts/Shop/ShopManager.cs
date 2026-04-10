using UnityEngine;
using UnityEngine.UI;

public class ShopManager : MonoBehaviour
{
    [SerializeField] private ShopData _shopData;
    [SerializeField] private ShopView _shopView;
    [SerializeField] private Button _rollButton;
    [SerializeField] private Button _levelUpButton;
    [SerializeField] private int _playerLevel = 1;
    [SerializeField] private int _maxPlayerLevel = 10;
    [SerializeField] private int _money = 100;

    private ShopPresenter _shopPresenter;
    private const int _minLevel = 1;
    private void Start()
    {
        var shopModel = new ShopModel(_shopData);
        _shopPresenter = new ShopPresenter(shopModel, _shopView);

        _rollButton.onClick.AddListener(OnClickRoll);
        _levelUpButton.onClick.AddListener(OnClickLevelUp);
    }

    private void OnClickRoll()
    {
        Debug.Log("クリックされた");
        
        _shopPresenter.Roll();
    }

    private void OnClickLevelUp()
    {
        if (_playerLevel >= _maxPlayerLevel)
        {
            Debug.Log("これ以上レベルアップできません");
            return;
        }

        int costIndex = Mathf.Clamp(_playerLevel-1, 0, _shopData.LevelUpCost.Length - 1);
        int cost = _shopData.LevelUpCost[costIndex];

        if (cost > _money)
        {
            Debug.Log("お金が足りません");
            return;
        }
        _playerLevel = Mathf.Min(_playerLevel + _minLevel, _maxPlayerLevel);
        _money -= cost; 
        Debug.Log("レベルアップ！現在のレベル: " + _playerLevel);
    }
}
