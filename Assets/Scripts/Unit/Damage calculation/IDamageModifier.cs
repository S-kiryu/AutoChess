//攻撃のダメージ計算に使用するインターフェース
public interface IDamageModifier
{
    void Apply(DamageContext context);
}