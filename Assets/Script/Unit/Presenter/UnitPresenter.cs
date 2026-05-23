public class UnitPresenter
{
    public UnitInstance Instance { get; }
    public UnitView View { get; }

    public UnitPresenter(UnitInstance instance, UnitView view)
    {
        Instance = instance;
        View = view;

        //‰Šú‰»‚µ‚Ä‚©‚çView‚ğXV‚·‚é
        View.Initialize(Instance);
        RefreshView();
    }

    public void RefreshView()
    {
        if (Instance == null || View == null)
        {
            return;
        }
    }
}