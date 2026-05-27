/// <summary>
/// ユニットのPresenterクラス
/// </summary>
public class UnitPresenter
{
    private readonly UnitInstance instance;
    private readonly UnitView view;

    public UnitPresenter(UnitInstance instance, UnitView view)
    {
        this.instance = instance;
        this.view = view;
    }

    public void RefreshView()
    {
        if (instance == null || view == null)
        {
            return;
        }

        view.UpdateView(instance.Data);
    }
}