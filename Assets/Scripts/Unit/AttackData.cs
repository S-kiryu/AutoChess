using UnityEngine;

public class AttackData
{
    public float Power;
    public DamageType Type;

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
