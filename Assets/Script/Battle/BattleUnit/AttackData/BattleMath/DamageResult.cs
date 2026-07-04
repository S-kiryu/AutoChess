public readonly struct DamageResult
{
    public readonly float Damage;
    public readonly bool IsCritical;
    public readonly bool IsDodged;

    public bool HasDamage => Damage > 0;

    /// <summary>
    /// 攻撃時の計算結果を格納する構造体
    /// </summary>
    /// <param name="damage">与えたダメージ量</param>
    /// <param name="isCritical">クリティカルしたかどうか</param>
    /// <param name="isDodged">回避したかどうか</param>
    public DamageResult(
        float damage,
        bool isCritical,
        bool isDodged)
    {
        Damage = damage;
        IsCritical = isCritical;
        IsDodged = isDodged;
    }

    public static DamageResult Zero =>
        new DamageResult(
            damage: 0,
            isCritical: false,
            isDodged: false);
}