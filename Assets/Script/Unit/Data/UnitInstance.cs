using System;
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
    public const int MaxStar = 3;
    public int Star => star;
    public bool CanUpgrade => star < MaxStar;


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
            return false;
        }

        star++;
        status.Initialize(data.BaseStatus);
        status.ApplyStar(star);

        return true;
    }
}