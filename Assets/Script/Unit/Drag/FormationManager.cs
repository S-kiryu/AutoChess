using UnityEngine;

public class FormationManager : MonoBehaviour
{
    [SerializeField] private int slotCount = 5;

    private UnitInstance[] slots;

    private void Awake()
    {
        slots = new UnitInstance[slotCount];
    }

    public void SetUnit(int slotIndex, UnitInstance unit)
    {
        if (slotIndex < 0 || slotIndex >= slots.Length)
        {
            return;
        }

        slots[slotIndex] = unit;
    }

    public UnitInstance GetUnit(int slotIndex)
    {
        if (slotIndex < 0 || slotIndex >= slots.Length)
        {
            return null;
        }

        return slots[slotIndex];
    }
}