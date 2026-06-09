using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Slot全体のUIを管理するクラス
/// </summary>
public class BenchUI : MonoBehaviour
{
    [SerializeField] private BenchManager benchManager;
    [SerializeField] private BenchSlotUI slotPrefab;
    [SerializeField] private Transform slotParent;
    [SerializeField] private GameObject backgroundPrefab;
    [SerializeField] private Transform backgroundParent;

    private BenchSlotUI[,] slotUIs;
    private int width, height;

    private void Awake()
    {
        if (benchManager == null)
        {
            Debug.LogError("BenchManagerが設定されていません。", this);
            enabled = false;
            return;
        }

        if (slotPrefab == null)
        {
            Debug.LogError("SlotPrefabが設定されていません。", this);
            enabled = false;
            return;
        }

        if (slotParent == null)
        {
            slotParent = transform;
        }
        
        width = benchManager.Width;
        height = benchManager.Height;

        slotUIs = new BenchSlotUI[width, height];

        GenerateBackground();
        BenchGenerate();
    }

    /// <summary>
    /// ベンチを生成する
    /// </summary>
    private void BenchGenerate()
    {
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                BenchSlotUI slotUI = Instantiate(slotPrefab, slotParent);
                slotUIs[x, y] = slotUI;

                slotUI.Clear();
            }
        }
    }
    private void GenerateBackground()
    {
        if (backgroundPrefab == null)
        {
            return;
        }

        Transform parent = backgroundParent != null ? backgroundParent : transform;
        Instantiate(backgroundPrefab, parent);
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