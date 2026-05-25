using UnityEngine;

public class UnitPresenter: MonoBehaviour
{
    [SerializeField]private UnitInstance Instance;
    [SerializeField]private UnitView View;

    public void Start()
    {
        RefreshView();
    }

    // Viewを更新するためのメソッド
    public void RefreshView()
    {
        if (Instance == null || View == null)
        {
            return;
        }
        View.UpdateView(Instance.Data);
    }
}