using UnityEngine;
using UnityEngine.EventSystems;

public class UnitItemDropTarget : MonoBehaviour, IDropHandler
{
    [SerializeField] private BenchSlotUI benchSlotUI;

    private void Awake()
    {
        if (benchSlotUI == null)
        {
            benchSlotUI = GetComponent<BenchSlotUI>();
        }
    }

    public void OnDrop(PointerEventData eventData)
    {
        ItemSlotUI itemSlotUI =
            eventData.pointerDrag?.GetComponent<ItemSlotUI>();

        if (itemSlotUI == null)
        {
            BenchSlot parentBenchSlot = GetComponentInParent<BenchSlot>();

            if (parentBenchSlot != null)
            {
                parentBenchSlot.OnDrop(eventData);
            }

            return;
        }

        if (GameLoopManager.Instance == null ||
            GameLoopManager.Instance.CurrentState != GameState.Preparation)
        {
            return;
        }

        if (benchSlotUI == null || benchSlotUI.Unit == null)
        {
            return;
        }

        if (itemSlotUI.Item == null)
        {
            return;
        }

        bool success =
            benchSlotUI.Unit.TryEquipOrRefineItem(itemSlotUI.Item);

        if (!success)
        {
            return;
        }

        benchSlotUI.RefreshItemIcons();

        ItemInventoryManager.Instance.RemoveItem(itemSlotUI.Item);
        itemSlotUI.SetDropped(true);
        Destroy(itemSlotUI.gameObject);
    }
}