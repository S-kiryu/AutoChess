using UnityEngine;

/// キャラクターデータを格納するScriptableObject
[CreateAssetMenu(menuName = "Character/CharacterData")]
public class CharacterData : ScriptableObject
{ 
    public string CharacterName;    //キャラの名前
    public Sprite Icon;             //キャラのアイコン
    public UnitAnimationData AnimationData;//アニメーションデータ
    public BaseStatus BaseStatus;   //キャラのベースステータス
    public NormalAttackData NormalAttack;//キャラの通常攻撃データ
    public SkillData Skill;      //キャラのスキルデータ
    [Header("レベルアップ設定")]
    public LevelUpStatusData[] LevelUpStatuses;
    [Header("星グレード設定")]
    public StarGradeData[] StarGrades;
}