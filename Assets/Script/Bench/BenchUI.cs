using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Slot全体のUIを管理するクラス
/// </summary>
public class BenchUI : MonoBehaviour
{
    [SerializeField] private BenchManager benchManager;
    [SerializeField] private BenchSlotUI[,] slotUIs;
    [SerializeField] private GameObject slotPrefab;
    [SerializeField] private Image _background;
    int width, height;

    private void Start()
    {
        width = benchManager.Width;
        height = benchManager.Height;
        BenchGenerate();
    }

    /// <summary>
    /// ベンチを生成する
    /// </summary>
    private void BenchGenerate()
    {
        for(int x = 0; x < width; x++)
        {
            for(int y = 0; y < height; y++)
            {
                slotUIs[x, y] = Instantiate(slotPrefab, transform).GetComponent<BenchSlotUI>();
                slotUIs[x, y].SetBackground(_background);
            }
        }
    }

    #region//イベント系
    //イベント追加
    private void OnEnable()
    {
        benchManager.OnUnitPlaced += HandleUnitPlaced;
        benchManager.OnUnitRemoved += HandleUnitRemoved;
    }

    //イベント削除
    private void OnDisable()
    {
        benchManager.OnUnitPlaced -= HandleUnitPlaced;
        benchManager.OnUnitRemoved -= HandleUnitRemoved;
    }

    // ユニットが配置されたときのUI更新
    private void HandleUnitPlaced(UnitInstance instance, int x, int y)
    {
        slotUIs[x, y].SetUnit(instance);
        slotUIs[x, y].PlayPlaceEffect();
    }

    // ユニットが撤去されたときのUI更新
    private void HandleUnitRemoved(UnitInstance instance, int x, int y)
    {
        slotUIs[x, y].Clear();
        slotUIs[x, y].PlayRemoveEffect();
    }
    #endregion
}