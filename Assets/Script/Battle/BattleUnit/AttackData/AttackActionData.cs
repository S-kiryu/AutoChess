using UnityEngine;

public abstract class AttackActionData : ScriptableObject
{
    [Header("Damage")]
    [SerializeField] private DamageType damageType = DamageType.Physical;
    [SerializeField] private float damageMultiplier = 1f;

    [Header("Range")]
    [SerializeField] private int castRange = 1;
    [SerializeField] private ActionRangeData rangeData;

    [Header("Hit")]
    [SerializeField] private int hitCount = 1;

    public DamageType DamageType => damageType;
    public float DamageMultiplier => damageMultiplier;
    public int CastRange => castRange;
    public int HitCount => hitCount;

    public ActionRangeData RangeData => rangeData;
}