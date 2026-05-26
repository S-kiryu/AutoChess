using UnityEngine;

/// キャラクターデータを格納するScriptableObject
[CreateAssetMenu(menuName = "Character/CharacterData")]
public class CharacterData : ScriptableObject
{ 
    public string CharacterName;    //キャラの名前
    public Sprite Icon;             //キャラのアイコン
    public BaseStatus BaseStatus;   //キャラのベースステータス
}