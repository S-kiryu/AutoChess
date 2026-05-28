using System.Collections.Generic;
using UnityEngine;

/// <summary>
///CharacterManagerのキャラクターリストを表示するためのUIクラス
/// </summary>
public class CharacterListView : MonoBehaviour
{
    [SerializeField] private CharacterIconView iconPrefab;
    [SerializeField] private Transform contentRoot;
    [SerializeField] private FormationManager formationManager;
    [SerializeField] private Transform containerRoot;
    [SerializeField] private RectTransform itemRootPrefab;             // アイコンを配置するための親オブジェクトのPrefab
    [SerializeField] private Vector2 cellSize = new Vector2(100, 100); // アイコンサイズ
    [SerializeField] private Vector2 spacing = new Vector2(10, 10);   // 間隔
    [SerializeField] private int columnCount = 4;                     // 横に何個並べるか
    [SerializeField] private bool centerAlign = true;                 // 中央寄せするか

    private readonly List<CharacterIconView> spawnedIcons = new();
    private readonly List<RectTransform> spawnedItems = new();

    private void OnEnable()
    {
        if (formationManager != null)
        {
            formationManager.OnFormationChanged += UpdateIconStates;
        }
    }

    private void OnDisable()
    {
        if (formationManager != null)
        {
            formationManager.OnFormationChanged -= UpdateIconStates;
        }
    }

    /// <summary>
    /// キャラクターを生成して表示するためのメソッド
    /// </summary>
    public void Refresh()
    {
        ClearSpawnedIcons();

        Transform parent =
            containerRoot != null ? containerRoot :
            contentRoot != null ? contentRoot :
            transform;

        if (iconPrefab == null) return;
        if (CharacterManager.Instance == null) return;
        if (columnCount <= 0) columnCount = 1;

        foreach (UnitInstance unit in CharacterManager.Instance.Characters)
        {
            RectTransform itemRoot;

            if (itemRootPrefab != null)
            {
                itemRoot = Instantiate(itemRootPrefab, parent, false);
            }
            else
            {
                itemRoot = new GameObject("ItemRoot", typeof(RectTransform)).GetComponent<RectTransform>();
                itemRoot.SetParent(parent, false);
            }

            CharacterIconView iconView = Instantiate(iconPrefab, itemRoot, false);
            iconView.Initialize(unit);

            spawnedItems.Add(itemRoot);
            spawnedIcons.Add(iconView);
        }

        LayoutIcons();
        UpdateIconStates();
    }

    private void LayoutIcons()
    {
        if (spawnedItems.Count == 0) return;
        if (columnCount <= 0) columnCount = 1;

        int count = spawnedItems.Count;

        for (int i = 0; i < count; i++)
        {
            RectTransform rect = spawnedItems[i];
            if (rect == null) continue;

            int row = i / columnCount;
            int col = i % columnCount;

            float x = col * (cellSize.x + spacing.x);
            float y = -row * (cellSize.y + spacing.y);

            if (centerAlign)
            {
                int columnsInThisRow = Mathf.Min(columnCount, count - row * columnCount);
                float totalWidth = columnsInThisRow * cellSize.x + (columnsInThisRow - 1) * spacing.x;
                x -= totalWidth / 2f - cellSize.x / 2f;
            }

            rect.anchorMin = new Vector2(0.5f, 1f);
            rect.anchorMax = new Vector2(0.5f, 1f);
            rect.pivot = new Vector2(0.5f, 1f);

            rect.anchoredPosition = new Vector2(x, y);
            rect.sizeDelta = cellSize;
        }
    }

    private RectTransform GetLayoutParent()
    {
        Transform parent =
            containerRoot != null ? containerRoot :
            contentRoot != null ? contentRoot :
            transform;

        return parent as RectTransform;
    }

    /// <summary>
    /// 編成に配置されているキャラクターのアイコンをグレーアウトするためのメソッド
    /// </summary>
    private void UpdateIconStates()
    {
        if (formationManager == null)
        {
            return;
        }

        foreach (CharacterIconView iconView in spawnedIcons)
        {
            if (iconView == null || iconView.Unit == null)
            {
                continue;
            }

            iconView.SetAssigned(formationManager.IsAssigned(iconView.Unit));
        }
    
    }

    /// <summary>
    /// 生成されたアイコンを削除するためのメソッド
    /// </summary>
    private void ClearSpawnedIcons()
    {
        foreach (RectTransform item in spawnedItems)
        {
            if (item != null)
            {
                Destroy(item.gameObject);
            }
        }

        spawnedItems.Clear();
        spawnedIcons.Clear();
    }
}