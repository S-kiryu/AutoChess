using UnityEngine;

[CreateAssetMenu(menuName = "AutoChess/Attack/NormalAttackData")]
public class NormalAttackData : ScriptableObject
{
    [Header("ダメージ")]
    [SerializeField] private DamageType damageType = DamageType.Physical;
    [SerializeField] private float damageMultiplier = 1f;

    [Header("前方攻撃範囲")]
    [SerializeField] private int width = 1;
    [SerializeField] private int depth = 1;

    public DamageType DamageType => damageType;
    public float DamageMultiplier => damageMultiplier;
    public int Width => width;
    public int Depth => depth;
}