using UnityEngine;

[CreateAssetMenu(fileName = "BaseStatus", menuName = "ScriptableObjects/BaseStatus", order = 1)]
public class BaseStatus : ScriptableObject
{
    public int Hp;
    public int Mp;
    public int Level;

    public int Attack;
    public int MagicAttack;
    public int Defense;
    public int MagicDefense;
    public int AttackSpeed;

    public int CriticalRate;
    public int CriticalDamage;

    public int AttackRange;
    public int Speed;
    public int Evasion;

    public int Cost;
    public int Rarity;
    //Œã‚ÅEnum‚ÅŽí‘°‚ð‚«‚ß‚ê‚é‚æ‚¤‚É‚·‚é
}
