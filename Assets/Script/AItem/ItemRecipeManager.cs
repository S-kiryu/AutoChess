using UnityEngine;

public class ItemRecipeManager : MonoBehaviour
{
    public static ItemRecipeManager Instance { get; private set; }

    [SerializeField] private ItemRecipe[] recipes;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

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
            if (recipe == null || !recipe.IsMatch(itemA, itemB))
            {
                continue;
            }

            completedItem = recipe.CompletedItem;
            return completedItem != null;
        }

        return false;
    }
}