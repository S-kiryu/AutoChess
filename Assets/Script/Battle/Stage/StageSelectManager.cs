using UnityEngine;
using UnityEngine.SceneManagement;

public class StageSelectManager : MonoBehaviour
{
    [SerializeField] private ChapterData[] chapters;
    [SerializeField] private string battleSceneName = "Battle";
    [SerializeField] private ChapterSelectButton buttonPrefab;
    [SerializeField] private Transform buttonRoot;

    public static ChapterData SelectedChapter { get; private set; }
    public static int SelectedBattleIndex { get; private set; }

    private void Start()
    {
        CreateButtons();
    }

    private void CreateButtons()
    {
        for (int i = 0; i < chapters.Length; i++)
        {
            ChapterSelectButton button =
                Instantiate(buttonPrefab, buttonRoot);

            button.Initialize(
                this,
                i,
                chapters[i],
                IsChapterUnlocked(i),
                IsChapterCleared(i));
        }
    }

    public ChapterData GetChapter(int chapterIndex)
    {
        if (chapterIndex < 0 || chapterIndex >= chapters.Length)
        {
            return null;
        }

        return chapters[chapterIndex];
    }

    public bool IsChapterUnlocked(int chapterIndex)
    {
        if (chapterIndex <= 0)
        {
            return true;
        }

        ChapterData previousChapter = GetChapter(chapterIndex - 1);
        return PlayerProgress.IsChapterCleared(previousChapter);
    }

    public bool IsChapterCleared(int chapterIndex)
    {
        return PlayerProgress.IsChapterCleared(GetChapter(chapterIndex));
    }

    public void SelectChapter(int chapterIndex)
    {
        ChapterData chapter = GetChapter(chapterIndex);

        if (chapter == null)
        {
            return;
        }

        if (!IsChapterUnlocked(chapterIndex))
        {
            Debug.Log("まだ解放されていません");
            return;
        }

        SelectedChapter = chapter;
        SelectedBattleIndex = 0;

        SceneManager.LoadScene(battleSceneName);
    }
}