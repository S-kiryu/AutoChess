using System.Collections.Generic;
using UnityEngine;

public class BattleActionRangeVisualizer : MonoBehaviour
{
    [SerializeField] private Color normalAttackRangeColor = Color.yellow;
    [SerializeField] private Color skillRangeColor = Color.magenta;
    [SerializeField] private float flashDuration = 0.25f;

    public void FlashRange(
        AttackActionData actionData,
        List<BattleGrid> targetGrids)
    {
        if (actionData == null || targetGrids == null)
        {
            return;
        }

        Color flashColor = actionData is SkillData
            ? skillRangeColor
            : normalAttackRangeColor;

        foreach (BattleGrid grid in targetGrids)
        {
            if (grid == null)
            {
                continue;
            }

            grid.FlashColor(flashColor, flashDuration);
        }
    }
}