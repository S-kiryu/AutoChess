using UnityEngine;

public abstract class AttackActionData : ScriptableObject
{
    [Header("Damage")]
    [SerializeField] private DamageType damageType = DamageType.Physical;
    [SerializeField] private float damageMultiplier = 1f;

    [Header("Range")]
    [SerializeField] private int width = 1;
    [SerializeField] private int depth = 1;

    [Header("Hit")]
    [SerializeField] private int hitCount = 1;

    public DamageType DamageType => damageType;
    public float DamageMultiplier => damageMultiplier;
    public int Width => width;
    public int Depth => depth;
    public int HitCount => hitCount;
}