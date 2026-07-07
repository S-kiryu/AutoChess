using UnityEngine;

public class StageSelectionContext : MonoBehaviour
{
    public static StageSelectionContext Instance { get; private set; }

    public ChapterData SelectedChapter { get; private set; }
    public int StartBattleIndex { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void SelectChapter(ChapterData chapter, int startBattleIndex = 0)
    {
        SelectedChapter = chapter;
        StartBattleIndex = startBattleIndex;
    }

    public bool HasSelection()
    {
        return SelectedChapter != null;
    }

    public void Clear()
    {
        SelectedChapter = null;
        StartBattleIndex = 0;
    }
}