using System;
using System.Collections.Generic;
using UnityEngine;

public class ItemInventoryManager : MonoBehaviour
{
    public static ItemInventoryManager Instance { get; private set; }

    private readonly List<ItemInstance> items = new List<ItemInstance>();

    public IReadOnlyList<ItemInstance> Items => items;
    public event Action OnItemsChanged;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    public void AddItem(ItemData itemData)
    {
        if (itemData == null)
        {
            Debug.LogWarning("追加する ItemData が設定されていません。");
            return;
        }

        items.Add(new ItemInstance(itemData));
        OnItemsChanged?.Invoke();
    }

    public bool RemoveItem(ItemInstance item)
    {
        bool removed = items.Remove(item);

        if (removed)
        {
            OnItemsChanged?.Invoke();
        }

        return removed;
    }
}