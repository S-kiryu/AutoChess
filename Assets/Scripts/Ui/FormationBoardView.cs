using System.Collections.Generic;
using UnityEngine;

/// <summary>編成ボードの表示を管理するクラス<summary>
public class FormationBoardView : MonoBehaviour
{
    [SerializeField] private FormationSlot slotPrefab;
    private List<FormationSlot> slots = new();

    public void Initialize(int width, int height)
    {
        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                var slot = Instantiate(slotPrefab, transform);
                slot.transform.localPosition = new Vector3(x, -y, 0);
                slots.Add(slot);
            }
        }
    }

    public void SetUnits(IReadOnlyList<UnitData> units)
    {
        for (int i = 0; i < slots.Count; i++)
        {
            UnitData data = i < units.Count ? units[i] : null;
            slots[i].SetUnit(data, i);
        }
    }
}
