using UnityEngine;

public class ItemInventoryUI : MonoBehaviour
{
    [SerializeField] private ItemInventoryManager inventoryManager;
    [SerializeField] private ItemSlotUI itemSlotPrefab;
    [SerializeField] private Transform slotRoot;
    [SerializeField] private Canvas canvas;

    private void Awake()
    {
        if (inventoryManager == null)
        {
            inventoryManager = ItemInventoryManager.Instance;
        }
    }

    private void OnEnable()
    {
        if (inventoryManager != null)
        {
            inventoryManager.OnItemsChanged += Refresh;
        }

        Refresh();
    }

    private void OnDisable()
    {
        if (inventoryManager != null)
        {
            inventoryManager.OnItemsChanged -= Refresh;
        }
    }

    private void Refresh()
    {
        if (inventoryManager == null ||
            itemSlotPrefab == null ||
            slotRoot == null)
        {
            return;
        }

        foreach (Transform child in slotRoot)
        {
            Destroy(child.gameObject);
        }

        foreach (ItemInstance item in inventoryManager.Items)
        {
            ItemSlotUI slot = Instantiate(itemSlotPrefab, slotRoot);
            slot.SetItem(item);
        }
    }
}