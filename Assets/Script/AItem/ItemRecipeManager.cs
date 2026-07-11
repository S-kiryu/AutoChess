using UnityEngine;

public class ItemRecipeManager : MonoBehaviour
{
    [SerializeField] private ItemRecipe[] recipes;

    public bool TryGetCompletedItem(
        ItemData itemA,
        ItemData itemB,
        out ItemData completedItem)
    {
        completedItem = null;

        if (itemA == null || itemB == null)
        {
            return false;
        }

        foreach (ItemRecipe recipe in recipes)
        {
            if (recipe == null)
            {
                continue;
            }

            if (!recipe.IsMatch(itemA, itemB))
            {
                continue;
            }

            completedItem = recipe.CompletedItem;
            return true;
        }

        return false;
    }
}