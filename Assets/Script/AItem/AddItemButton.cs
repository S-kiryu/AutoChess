using UnityEngine;

public class AddItemButton : MonoBehaviour
{
    [SerializeField] private ItemInventoryManager inventoryManager;
    [SerializeField] private ItemData itemData;

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

        inventoryManager.AddItem(itemData);
    }
}