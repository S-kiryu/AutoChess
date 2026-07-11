using System;
using UnityEngine;

[Serializable]
public class ItemRecipe
{
    [SerializeField] private ItemData materialA;
    [SerializeField] private ItemData materialB;
    [SerializeField] private ItemData completedItem;

    public ItemData MaterialA => materialA;
    public ItemData MaterialB => materialB;
    public ItemData CompletedItem => completedItem;

    public bool IsMatch(ItemData itemA, ItemData itemB)
    {
        if (itemA == null || itemB == null)
        {
            return false;
        }

        return
            itemA == materialA && itemB == materialB ||
            itemA == materialB && itemB == materialA;
    }
}