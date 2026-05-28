using System;
using UnityEngine;

//編成中のユニットを管理するクラス
public class FormationManager : MonoBehaviour
{
    [SerializeField] private int slotCount = 5;

    private UnitInstance[] slots;

    //編成が変更されたときに通知するためのイベント
    public event Action OnFormationChanged;

    private void Awake()
    {
        slots = new UnitInstance[slotCount];
    }

    //編成にユニットを配置するためのメソッド
    public bool SetUnit(int slotIndex, UnitInstance unit)
    {
        if (slotIndex < 0 || slotIndex >= slots.Length || unit == null)
        {
            return false;
        }

        int assignedIndex = GetAssignedSlotIndex(unit);

        if (assignedIndex >= 0 && assignedIndex != slotIndex)
        {
            return false;
        }

        if (slots[slotIndex] == unit)
        {
            return true;
        }

        slots[slotIndex] = unit;
        OnFormationChanged?.Invoke();
        return true;
    }

    //編成中のユニットを取得するためのメソッド
    public UnitInstance GetUnit(int slotIndex)
    {
        if (slotIndex < 0 || slotIndex >= slots.Length)
        {
            return null;
        }

        return slots[slotIndex];
    }

    //指定したユニットが編成に配置されているかを確認するためのメソッド
    public bool IsAssigned(UnitInstance unit)
    {
        return GetAssignedSlotIndex(unit) >= 0;
    }

    //指定したユニットがどこのスロットに配置されているかを確認するためのメソッド
    private int GetAssignedSlotIndex(UnitInstance unit)
    {
        for (int i = 0; i < slots.Length; i++)
        {
            if (slots[i] == unit)
            {
                return i;
            }
        }

        return -1;
    }
}