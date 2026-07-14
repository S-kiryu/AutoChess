using UnityEngine;
[CreateAssetMenu(
    fileName = "NewItem",
    menuName = "AutoChess/Item/ItemData")]
public class ItemData : ScriptableObject
{
    [Header("基本情報")]
    [SerializeField] private string itemName;
    [SerializeField] private Sprite icon;

    [TextArea]
    [SerializeField] private string description;

    [SerializeField] private ItemCategory category;
    public ItemCategory Category => category;

    [Header("加算ステータス")]
    [SerializeField] private int addMaxHp;
    [SerializeField] private int addAttack;
    [SerializeField] private int addMagicAttack;
    [SerializeField] private int addDefense;
    [SerializeField] private int addMagicDefense;
    [SerializeField] private float addAttackSpeed;
    [SerializeField] private float addCriticalRate;

    public string ItemName => itemName;
    public Sprite Icon => icon;
    public string Description => description;

    public int AddMaxHp => addMaxHp;
    public int AddAttack => addAttack;
    public int AddMagicAttack => addMagicAttack;
    public int AddDefense => addDefense;
    public int AddMagicDefense => addMagicDefense;
    public float AddAttackSpeed => addAttackSpeed;
    public float AddCriticalRate => addCriticalRate;

    /// <summary>
    /// 装備した瞬間に呼ばれる
    /// </summary>
    public virtual void OnEquip(UnitStatus owner)
    {
    }

    /// <summary>
    /// 装備を外した瞬間に呼ばれる
    /// </summary>
    public virtual void OnUnequip(UnitStatus owner)
    {
    }

    /// <summary>
    /// 戦闘開始時に呼ばれる
    /// </summary>
    public virtual void OnBattleStart(UnitStatus owner)
    {
    }

    /// <summary>
    /// 通常攻撃したときに呼ばれる
    /// </summary>
    public virtual void OnNormalAttack(
        UnitStatus owner,
        UnitStatus target)
    {
    }

    ///// <summary>
    ///// ダメージを与えたときに呼ばれる
    ///// </summary>
    //public virtual void OnDealDamage(
    //    UnitStatus owner,
    //    UnitStatus target,
    //    ref DamageData damage)
    //{
    //}

    /// <summary>
    /// ダメージを受ける直前に呼ばれる
    /// </summary>
    //public virtual void OnBeforeTakeDamage(
    //    UnitStatus owner,
    //    UnitStatus attacker,
    //    ref DamageData damage)
    //{
    //}

    ///// <summary>
    ///// ダメージを受けたあとに呼ばれる
    ///// </summary>
    //public virtual void OnAfterTakeDamage(
    //    UnitStatus owner,
    //    UnitStatus attacker,
    //    DamageData damage)
    //{
    //}

    /// <summary>
    /// スキルを使用したときに呼ばれる
    /// </summary>
    public virtual void OnSkillUsed(UnitStatus owner)
    {
    }

    /// <summary>
    /// 敵を倒したときに呼ばれる
    /// </summary>
    public virtual void OnKill(
        UnitStatus owner,
        UnitStatus target)
    {
    }

    /// <summary>
    /// 所有者が死亡したときに呼ばれる
    /// </summary>
    public virtual void OnOwnerDeath(UnitStatus owner)
    {
    }

    /// <summary>
    /// 戦闘終了時に呼ばれる
    /// </summary>
    /// <param name="owner"></param>
    public virtual void OnEndBattle(UnitStatus owner)
    {
    }
}