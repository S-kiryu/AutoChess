using System.ComponentModel;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Slot全体のUIを管理するクラス
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

        if (slotUIPrefab == null)
        {
            Debug.LogError("SlotPrefabが設定されていません。", this);
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
        slotUIs = new BenchSlotUI[width, height];

        GenerateSlots();
    }

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
    //スロットを生成
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

    private void GenerateUnit(int x, int y)
    {
        BenchSlot slot = slots[x, y];

        BenchSlotUI slotUI = Instantiate(slotUIPrefab, slot.transform);
        slotUI.SetCanvas(canvas);

        RectTransform rectTransform = slotUI.GetComponent<RectTransform>();

        if (rectTransform != null)
        {
            rectTransform.anchorMin = new Vector2(0.5f, 0.5f);
            rectTransform.anchorMax = new Vector2(0.5f, 0.5f);
            rectTransform.pivot = new Vector2(0.5f, 0.5f);
            rectTransform.anchoredPosition = Vector2.zero;
            rectTransform.localScale = Vector3.one;
        }
        else
        {
            slotUI.transform.localPosition = Vector3.zero;
            slotUI.transform.localScale = Vector3.one;
        }

        slotUI.Clear();

        slotUIs[x, y] = slotUI;
    }

    //ユニットを置くときにイベントで呼ぶ
    private void HandleUnitPlaced(UnitInstance instance, int x, int y)
    {
        if (slotUIs[x, y] == null)
        {
            GenerateUnit(x, y);
        }

        slotUIs[x, y].SetUnit(instance);
        slotUIs[x, y].Initialize(x, y);
        slotUIs[x, y].PlayPlaceEffect();
    }

    //イベントでユニットを消す
    private void HandleUnitRemoved(UnitInstance instance, int x, int y)
    {
        if (slotUIs[x, y] == null) return;

        Destroy(slotUIs[x, y].gameObject);
        slotUIs[x, y] = null;
    }

}