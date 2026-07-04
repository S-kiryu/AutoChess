using UnityEngine;

[CreateAssetMenu(menuName = "AutoChess/Skill/UnitSkillData")]
public class UnitSkillData : ScriptableObject
{
    [Header("Damage")]
    [SerializeField] private DamageType damageType = DamageType.Magic;
    [SerializeField] private float damageMultiplier = 2f;

    public DamageType DamageType => damageType;
    public float DamageMultiplier => damageMultiplier;
}