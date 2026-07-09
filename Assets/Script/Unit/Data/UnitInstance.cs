using System;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
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

        star = Mathf.Clamp(newStar, 1, data.StarGrades.Length);
        RecalculateStatus();
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
            return false;
        }

        level++;
        RecalculateStatus();
        return true;
    }

    public void RecalculateStatus()
    {
        status.Initialize(data.BaseStatus);

        if (data.StarGrades != null && data.StarGrades.Length > 0)
        {
            int starIndex = Mathf.Clamp(star - 1, 0, data.StarGrades.Length - 1);
            status.ApplyStar(data.StarGrades[starIndex]);
        }

        if (data.LevelUpStatuses != null && data.LevelUpStatuses.Length > 0)
        {
            int applyCount = Mathf.Min(level - 1, data.LevelUpStatuses.Length);

            for (int i = 0; i < applyCount; i++)
            {
                status.ApplyLevelUp(data.LevelUpStatuses[i], i + 2);
                Debug.Log($"{status.MaxHp}HP,{status.Attack}");
            }
        }
    }
}