public class ShopPresenter
{
    private ShopModel _model;
    private ShopView _view;

    private UnitData currentUnits;

    public ShopPresenter(ShopModel model, ShopView view)
    {
        this._model = model;
        this._view = view;

        Roll();
    }

    // ショップのユニットを更新するメソッド
    public void Roll()
    {
            currentUnits = _model.GetRandomUnit();
        _view.UpdateUnitIcons(currentUnits);
    }
}