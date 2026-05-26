using UnityEngine;

public class CharacterListView : MonoBehaviour
{
    [SerializeField] private CharacterIconView iconPrefab;
    [SerializeField] private Transform contentRoot;

    public void Refresh()
    {
        foreach (UnitInstance unit in CharacterManager.Instance.Characters)
        {
            CharacterIconView iconView = Instantiate(iconPrefab, contentRoot);
            iconView.Initialize(unit);
        }
    }
}