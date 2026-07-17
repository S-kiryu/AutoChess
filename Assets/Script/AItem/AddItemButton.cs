using UnityEngine;

public class AddItemButton : MonoBehaviour
{
    [SerializeField] private ItemInventoryManager inventoryManager;
    [SerializeField] private ItemData[] itemDatas;

    private void Awake()
    {
        if (inventoryManager == null)
        {
            inventoryManager = ItemInventoryManager.Instance;
        }
    }

    public void OnClickAddItem()
    {
        if (inventoryManager == null)
        {
            Debug.LogWarning("ItemInventoryManager が見つかりません。");
            return;
        }

        var ItemData = itemDatas[Random.Range(0, itemDatas.Length)];
        inventoryManager.AddItem(ItemData);
    }
}