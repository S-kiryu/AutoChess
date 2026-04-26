//戦闘中のダメージ計算に必要な情報をまとめるクラス
public class DamageContext
{
    public UnitModel Attacker;
    public UnitModel Target;
    public AttackData Attack;

    public float Power;
    public float FinalDamage;

    public bool IsCritical;
}