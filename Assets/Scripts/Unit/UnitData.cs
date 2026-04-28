using UnityEngine;

[System.Serializable]
public class UnitData
{
    [Header("基本ステータス")]public BaseStatus baseStatus;
    [Header("プレハブ")]public GameObject prefab;
    [Header("イメージ")]public Sprite Icon;
    [Header("サブイメージ")]public Sprite SubIcon;
}
