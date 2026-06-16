using UnityEngine;

/// <summary>
/// ステージの管理をするクラス
/// </summary>
public class StageProgressManager : MonoBehaviour
{
    [SerializeField] private ChapterData currentChapter;

    private int currentBattleIndex = 0;

    public BattleStageData CurrentBattleStage
    {
        get
        {
            return currentChapter.BattleStages[currentBattleIndex];
        }
    }

    /// <summary>
    /// 次のステージに行くための関数
    /// </summary>
    public void NextBattleStage()
    {
        currentBattleIndex++;

        if (currentBattleIndex >= currentChapter.BattleStages.Length)
        {
            Debug.Log("このチャプターをクリアしました");
            currentBattleIndex = currentChapter.BattleStages.Length - 1;
            return;
        }

        Debug.Log($"次のステージ: {CurrentBattleStage.StageId}");
    }
}