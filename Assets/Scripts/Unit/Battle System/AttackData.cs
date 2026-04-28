public class AttackData
{
    public float Power { get; }
    public DamageType Type { get; }

    public AttackData(float power, DamageType type)
    {
        Power = power;
        Type = type;
    }
}

public enum DamageType
{
    Physical,
    Magical
}
