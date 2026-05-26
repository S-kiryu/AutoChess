using UnityEngine;

public class OwnedCharacterInitializer : MonoBehaviour
{
    [SerializeField] private CharacterData[] initialCharacters;
    [SerializeField] private CharacterListView characterListView;

    private void Start()
    {
        OnCharacterListChanged();
    }

    //マネージャークラスにキャラのデータを渡して、キャラのリストを更新する
    private void OnCharacterListChanged()
    {
        foreach (CharacterData data in initialCharacters)
        {
            CharacterManager.Instance.CreateCharacter(data);
        }

        characterListView.Refresh();
    }
}