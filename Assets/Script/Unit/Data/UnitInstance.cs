using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 実態クラス
/// </summary>
[Serializable]
public class UnitInstance
{
    [SerializeField] private string uniqueId;
    [SerializeField] private CharacterData data;
    [SerializeField] private UnitStatus status = new UnitStatus();
    [SerializeField] private int star = 1;
    [SerializeField] private int level = 1;
    [SerializeField] private ItemInstance[] equippedItems = new ItemInstance[3];
    public event Action OnItemsChanged;

    public int MaxStar
    {
        get
        {
            if (data == null || data.StarGrades == null || data.StarGrades.Length == 0)
            {
                return 1;
            }

            return data.StarGrades.Length;
        }
    }

    public bool CanUpgrade => star < MaxStar;
    public int Star => star;

    public int Level => level;
    public bool CanLevelUp =>
        data != null &&
        data.LevelUpStatuses != null &&
        level <= data.LevelUpStatuses.Length;


    public string UniqueId => uniqueId;
    public CharacterData Data => data;
    public UnitStatus Status => status;
    public event Action<int> OnStarChanged;
    public IReadOnlyList<ItemInstance> EquippedItems => equippedItems;

    public void Initialize(CharacterData characterData)
    {
        if (characterData == null)
        {
            Debug.LogError("CharacterDataがないよ");
            return;
        }

        uniqueId = Guid.NewGuid().ToString();
        data = characterData;
        status.Initialize(characterData.BaseStatus);
    }

    public bool Upgrade()
    {
        if (!CanUpgrade)
        {
            Debug.Log("これ以上星を上げられません");
            return false;
        }

        SetStar(star + 1);
        return true;
    }

    public void SetStar(int newStar)
    {
        if (data == null || data.StarGrades == null || data.StarGrades.Length == 0)
        {
            return;
        }

        int clampedStar = Mathf.Clamp(newStar, 1, data.StarGrades.Length);

        if (star == clampedStar)
        {
            return;
        }

        star = clampedStar;
        RecalculateStatus();

        OnStarChanged?.Invoke(star);
    }

    public void SetLevel(int newLevel)
    {
        level = Mathf.Max(1, newLevel);
        RecalculateStatus();
    }

    public bool LevelUp()
    {
        if (!CanLevelUp)
        {
            Debug.LogWarning($"[LevelUp] {Data.CharacterName} cannot level up. Lv:{level} LevelUpStatuses:{data?.LevelUpStatuses?.Length ?? 0}");
            return false;
        }

        int beforeLevel = level;
        int beforeMaxHp = status.MaxHp;
        float beforeAttack = status.Attack;

        level++;
        RecalculateStatus();

        Debug.Log($"[LevelUp] {Data.CharacterName} Lv {beforeLevel} -> {level} / HP {beforeMaxHp} -> {status.MaxHp} / ATK {beforeAttack} -> {status.Attack}");
        return true;
    }

    public void RecalculateStatus()
    {
        float hpRate = status.MaxHp > 0
            ? (float)status.CurrentHp / status.MaxHp
            : 1f;

        int currentMp = status.CurrentMp;

        status.Initialize(data.BaseStatus);

        if (data.StarGrades != null && data.StarGrades.Length > 0)
        {
            int starIndex = Mathf.Clamp(star - 1, 0, data.StarGrades.Length - 1);
            status.ApplyStar(data.StarGrades[starIndex]);
        }

        if (data.LevelUpStatuses != null && data.LevelUpStatuses.Length > 0)
        {
            int applyCount = Mathf.Min(level - 1, data.LevelUpStatuses.Length);
            Debug.Log($"[RecalculateStatus] {data.CharacterName} Lv:{level} Star:{star} LevelUpStatuses:{data.LevelUpStatuses.Length} ApplyCount:{applyCount}");

            for (int i = 0; i < applyCount; i++)
            {
                LevelUpStatusData levelData = data.LevelUpStatuses[i];
                int beforeMaxHp = status.MaxHp;
                float beforeAttack = status.Attack;

                status.ApplyLevelUp(levelData, i + 2);

                Debug.Log($"[ApplyLevelUp] {data.CharacterName} Index:{i} TargetLv:{i + 2} AddHp:{levelData.AddHp} AddAtk:{levelData.AddAttack} / HP {beforeMaxHp} -> {status.MaxHp} / ATK {beforeAttack} -> {status.Attack}");
            }
        }
        else
        {
            Debug.LogWarning($"[RecalculateStatus] {data.CharacterName} has no LevelUpStatuses.");
        }

        status.SetLevel(level);
        status.RestoreCurrentHpRate(hpRate);
        status.SetCurrentMp(currentMp);

        // 装備アイテムのステータスを適用
        foreach (ItemInstance item in equippedItems)
        {
            if (item?.Data == null)
            {
                continue;
            }

            status.ApplyItem(item.Data);
        }
    }

    /// <summary>
    /// 装備アイテムを装備する
    /// </summary>
    /// <param name="item"></param>
    /// <returns></returns>
    public bool EquipItem(ItemInstance item)
    {
        if (item == null || item.Data == null)
        {
            return false;
        }

        for (int i = 0; i < equippedItems.Length; i++)
        {
            if (equippedItems[i] != null)
            {
                continue;
            }

            equippedItems[i] = item;
            RecalculateStatus();

            item.Data.OnEquip(status);
            OnItemsChanged?.Invoke();

            return true;
        }

        return false;
    }

    public bool TryEquipOrRefineItem(ItemInstance item)
    {
        return TryEquipOrRefineItem(item, ItemRecipeManager.Instance);
    }

    public bool TryEquipOrRefineItem(
        ItemInstance item,
        ItemRecipeManager recipeManager)
    {
        if (item == null || item.Data == null)
        {
            return false;
        }

        if (recipeManager != null && item.Data.Category == ItemCategory.Component)
        {
            for (int i = 0; i < equippedItems.Length; i++)
            {
                ItemInstance equippedItem = equippedItems[i];

                if (equippedItem?.Data == null ||
                    equippedItem.Data.Category != ItemCategory.Component)
                {
                    continue;
                }

                if (!recipeManager.TryGetCompletedItem(
                        equippedItem.Data,
                        item.Data,
                        out ItemData completedItem))
                {
                    continue;
                }

                equippedItems[i].Data.OnUnequip(status);
                equippedItems[i] = new ItemInstance(completedItem);

                RecalculateStatus();
                completedItem.OnEquip(status);
                OnItemsChanged?.Invoke();

                return true;
            }
        }

        return EquipItem(item);
    }

    public bool UnequipItem(int index, out ItemInstance removedItem)
    {
        removedItem = null;

        if (index < 0 || index >= equippedItems.Length)
        {
            return false;
        }

        removedItem = equippedItems[index];

        if (removedItem == null)
        {
            return false;
        }

        removedItem.Data?.OnUnequip(status);
        equippedItems[index] = null;

        RecalculateStatus();
        OnItemsChanged?.Invoke();

        return true;
    }
}
