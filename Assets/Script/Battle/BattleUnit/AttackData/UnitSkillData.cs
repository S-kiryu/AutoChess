using UnityEngine;

[CreateAssetMenu(menuName = "AutoChess/Skill/SkillData")]
public class SkillData : AttackActionData
{
    [Header("Info")]
    [SerializeField] private string skillName;
    [TextArea]
    [SerializeField] private string description;

    [Header("Mana")]
    [SerializeField] private int manaCost = 100;

    public string SkillName => skillName;
    public string Description => description;
    public int ManaCost => manaCost;
}