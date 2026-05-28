using System.Collections.Generic;
using UnityEngine;

/// <summary>
///CharacterManagerのキャラクターリストを表示するためのUIクラス
/// </summary>
public class CharacterListView : MonoBehaviour
{
    [SerializeField] private CharacterIconView iconPrefab;
    [SerializeField] private Transform contentRoot;
    [SerializeField] private FormationManager formationManager;

    private readonly List<CharacterIconView> spawnedIcons = new();

    private void OnEnable()
    {
        if (formationManager != null)
        {
            formationManager.OnFormationChanged += UpdateIconStates;
        }
    }

    private void OnDisable()
    {
        if (formationManager != null)
        {
            formationManager.OnFormationChanged -= UpdateIconStates;
        }
    }

    //キャラクターリストを更新するためのメソッド
    public void Refresh()
    {
        ClearSpawnedIcons();

        Transform parent = contentRoot != null ? contentRoot : transform;

        foreach (UnitInstance unit in CharacterManager.Instance.Characters)
        {
            CharacterIconView iconView = Instantiate(iconPrefab, parent, false);
            iconView.Initialize(unit);
            spawnedIcons.Add(iconView);
        }

        UpdateIconStates();
    }

    //編成に配置されているキャラクターのアイコンをグレーアウトするためのメソッド
    private void UpdateIconStates()
    {
        if (formationManager == null)
        {
            return;
        }

        foreach (CharacterIconView iconView in spawnedIcons)
        {
            if (iconView == null || iconView.Unit == null)
            {
                continue;
            }

            iconView.SetAssigned(formationManager.IsAssigned(iconView.Unit));
        }
    }

    //生成されたアイコンを削除するためのメソッド
    private void ClearSpawnedIcons()
    {
        foreach (CharacterIconView iconView in spawnedIcons)
        {
            if (iconView != null)
            {
                Destroy(iconView.gameObject);
            }
        }

        spawnedIcons.Clear();
    }
}