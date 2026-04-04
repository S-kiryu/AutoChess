using UnityEngine;

[CreateAssetMenu(fileName = "BaseStatus", menuName = "ScriptableObjects/BaseStatus", order = 1)]
public class BaseStatus : ScriptableObject
{
    public int Hp;
    public int Mp;
    public int Level;

    public float Attack;
    public float MagicAttack;
    public float Defense;
    public float MagicDefense;
    public int AttackSpeed;

    public float CriticalRate;
    public float CriticalDamage;

    public int AttackRange;
    public float Speed;
    //‰ñ”ð—¦
    public float dodgeRate;

    public int Cost;
    public int Rarity;
    public UnitType[] Type;
}


public enum UnitType
{
    Human,
    Elf,
    Orc,
    Undead,
    Beast
}
