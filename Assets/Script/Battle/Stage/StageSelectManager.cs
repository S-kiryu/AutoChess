using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class StageSelectManager : MonoBehaviour
{
    [Header("Chapter")]
    [SerializeField] private ChapterData[] chapters;

    [Header("Scene")]
    [SerializeField] private string battleSceneName = "Battle";

    [Header("UI")]
    [SerializeField] private Image chapterImage;
    [SerializeField] private TMP_Text chapterNameText;
    [SerializeField] private GameObject lockObject;
    [SerializeField] private Button previousButton;
    [SerializeField] private Button nextButton;
    [SerializeField] private Button startButton;

    public static ChapterData SelectedChapter { get; private set; }
    public static int SelectedBattleIndex { get; private set; }

    private int currentChapterIndex;

    private void Start()
    {
        currentChapterIndex = 0;

        previousButton.onClick.AddListener(PreviousChapter);
        nextButton.onClick.AddListener(NextChapter);
        startButton.onClick.AddListener(StartSelectedChapter);

        UpdateView();
    }

    private void PreviousChapter()
    {
        if (chapters == null || chapters.Length == 0)
        {
            return;
        }

        currentChapterIndex--;

        if (currentChapterIndex < 0)
        {
            currentChapterIndex = chapters.Length - 1;
        }

        UpdateView();
    }

    private void NextChapter()
    {
        if (chapters == null || chapters.Length == 0)
        {
            return;
        }

        currentChapterIndex++;

        if (currentChapterIndex >= chapters.Length)
        {
            currentChapterIndex = 0;
        }

        UpdateView();
    }

    private void StartSelectedChapter()
    {
        ChapterData chapter = GetCurrentChapter();

        if (chapter == null)
        {
            return;
        }

        if (!IsChapterUnlocked(currentChapterIndex))
        {
            Debug.Log("まだ解放されていません");
            return;
        }

        SelectedChapter = chapter;
        SelectedBattleIndex = 0;

        SceneManager.LoadScene(battleSceneName);
    }

    private void UpdateView()
    {
        ChapterData chapter = GetCurrentChapter();

        if (chapter == null)
        {
            chapterImage.sprite = null;
            chapterNameText.text = "";
            lockObject.SetActive(false);
            startButton.interactable = false;
            return;
        }

        bool unlocked = IsChapterUnlocked(currentChapterIndex);

        chapterImage.sprite = chapter.ChapterImage;
        chapterNameText.text = chapter.ChapterName;
        lockObject.SetActive(!unlocked);
        startButton.interactable = unlocked;
    }

    private ChapterData GetCurrentChapter()
    {
        if (chapters == null || chapters.Length == 0)
        {
            return null;
        }

        if (currentChapterIndex < 0 ||
            currentChapterIndex >= chapters.Length)
        {
            return null;
        }

        return chapters[currentChapterIndex];
    }

    private bool IsChapterUnlocked(int chapterIndex)
    {
        if (chapterIndex <= 0)
        {
            return true;
        }

        ChapterData previousChapter = chapters[chapterIndex - 1];
        return PlayerProgress.IsChapterCleared(previousChapter);
    }
}