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

    private void Awake()
    {
        if (StageSelectManager.SelectedChapter != null)
        {
            Initialize(
                StageSelectManager.SelectedChapter,
                StageSelectManager.SelectedBattleIndex);
            return;
        }

        if (currentChapter != null)
        {
            Initialize(currentChapter, currentBattleIndex);
        }
    }

    public void Initialize(ChapterData chapter, int battleIndex)
    {
        if (chapter == null ||
            chapter.BattleStages == null ||
            chapter.BattleStages.Length == 0)
        {
            Debug.LogError("ChapterData が正しくありません。");
            return;
        }

        currentChapter = chapter;
        currentBattleIndex = Mathf.Clamp(
            battleIndex,
            0,
            currentChapter.BattleStages.Length - 1);

        chapterClearPending = false;
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
}