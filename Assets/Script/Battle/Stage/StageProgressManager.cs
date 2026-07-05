using UnityEngine;

/// <summary>
/// ステージの管理をするクラス
/// </summary>
public class StageProgressManager : MonoBehaviour
{
    [SerializeField] private ChapterData currentChapter;

    private int currentBattleIndex = 0;
    private bool chapterClearPending;

    public bool ChapterClearPending => chapterClearPending;
    public ChapterData CurrentChapter => currentChapter;
    public int CurrentBattleIndex => currentBattleIndex;

    public BattleStageData CurrentBattleStage
    {
        get
        {
            return currentChapter.BattleStages[currentBattleIndex];
        }
    }


    public void MarkChapterClear()
    {
        chapterClearPending = true;
    }

    public void ClearChapterClearPending()
    {
        chapterClearPending = false;
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

    public void Reward() 
    {

    }
}