public class ShopPresenter
{
    private ShopModel _model;
    private ShopView _view;

    private UnitData[] currentUnits;

    public ShopPresenter(ShopModel model, ShopView view)
    {
        this._model = model;
        this._view = view;

        Roll(1);//仮で1Lvを入れてる
    }

    // プレイヤーレベルに応じてショップのユニットを更新するメソッド
    public void Roll(int playerLevel)
    {
        currentUnits = new UnitData[5];

        for (int i = 0; i < 5; i++)
        {
            currentUnits[i] = _model.GetRandomUnit(playerLevel);
        }

        _view.UpdateUnitIcons(currentUnits);
    }
}