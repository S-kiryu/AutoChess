using UnityEngine;

[System.Serializable]
public class UnitData
{
    [Header("基本ステータス"),SerializeField] private BaseStatus baseStatus;
    [Header("イメージ"), SerializeField] private Sprite icon;

    public BaseStatus BaseStatus => baseStatus;
    public Sprite Icon => icon;
}
