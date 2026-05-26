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

    public string UniqueId => uniqueId;
    public CharacterData Data => data;
    public UnitStatus Status => status;

    public void Initialize(CharacterData characterData)
    {
        if (characterData == null)
        {
            Debug.LogError("CharacterData is null.");
            return;
        }

        uniqueId = Guid.NewGuid().ToString();
        data = characterData;
        status.Initialize(characterData.BaseStatus);
    }
}