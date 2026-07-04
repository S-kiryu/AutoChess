using UnityEngine;

public static class UnitHeal
{
    /// <summary>
    /// ユニットのHPを回復するメソッド
    /// 後々回復疎外などがある可能性があるため
    /// </summary>
    /// <param name="unit"></param>
    /// <param name="healAmount"></param>
    public static void HealUnit(UnitStatus unit, float healAmount)
    {
        if (unit == null)
        {
            Debug.LogError("UnitStatus is null.");
            return;
        }
        unit.Heal(healAmount);
        Debug.Log($"{healAmount} HP. Current HP: {unit.CurrentHp}/{unit.MaxHp}");
    }
}
