using UnityEngine;

/// <summary>
/// ベンチのスロットと、新規購入時のユニットUI生成を管理する。
/// </summary>
public class BenchUI : MonoBehaviour
{
    [SerializeField] private BenchManager benchManager;

    [Header("生成するプレハブ")]
    [SerializeField] private BenchSlot slotPrefab;
    [SerializeField] private BenchSlotUI slotUIPrefab;
    [SerializeField] private Canvas canvas;
    [SerializeField] private Transform slotRoot;

    private BenchSlot[,] slots;
    private int width;
    private int height;

    private void Awake()
    {
        if (benchManager == null ||
            slotPrefab == null ||
            slotUIPrefab == null)
        {
            Debug.LogError("BenchUIの参照が設定されていません。", this);
            enabled = false;
            return;
        }

        if (slotRoot == null)
        {
            slotRoot = transform;
        }

        width = benchManager.Width;
        height = benchManager.Height;

        slots = new BenchSlot[width, height];

        GenerateSlots();
    }

    private void OnEnable()
    {
        if (benchManager == null)
        {
            return;
        }

        benchManager.OnUnitPlaced += HandleUnitPlaced;
        benchManager.OnUnitRemoved += HandleUnitRemoved;
    }

    private void OnDisable()
    {
        if (benchManager == null)
        {
            return;
        }

        benchManager.OnUnitPlaced -= HandleUnitPlaced;
        benchManager.OnUnitRemoved -= HandleUnitRemoved;
    }

    /// <summary>
    /// ベンチを生成する
    /// </summary>
    private void GenerateSlots()
    {
        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                BenchSlot slot = Instantiate(slotPrefab, slotRoot);

                slot.name = $"BenchSlot_{x}_{y}";
                slot.Initialize(x, y, benchManager);

                slots[x, y] = slot;
            }
        }
    }

    /// <summary>
    /// ユニットが生成時にいろいろと設定するメソット
    /// </summary>
    private void HandleUnitPlaced(
        UnitInstance unit,
        int x,
        int y)
    {
        if (!benchManager.IsInside(x, y))
        {
            return;
        }

        BenchSlot slot = slots[x, y];

        BenchSlotUI existingUI =
            slot.GetComponentInChildren<BenchSlotUI>(true);

        if (existingUI != null)
        {
            existingUI.SetUnit(unit);
            existingUI.SetLocation(UnitArea.Bench, x, y);
            return;
        }

        BenchSlotUI unitUI =
            Instantiate(slotUIPrefab, slot.transform);

        unitUI.SetCanvas(canvas);
        unitUI.SetUnit(unit);
        unitUI.SetLocation(UnitArea.Bench, x, y);
        unitUI.MoveTo(slot.transform);
        unitUI.PlayPlaceEffect();
    }

    /// <summary>
    /// 売却時だけUIオブジェクトを破棄する。
    /// </summary>
    private void HandleUnitRemoved(
        UnitInstance unit,
        int x,
        int y)
    {
        if (!benchManager.IsInside(x, y))
        {
            return;
        }

        BenchSlotUI unitUI =
            slots[x, y].GetComponentInChildren<BenchSlotUI>(true);

        if (unitUI == null)
        {
            return;
        }

        unitUI.PlayRemoveEffect();
        Destroy(unitUI.gameObject);
    }
}