using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ChapterSelectButton : MonoBehaviour
{
    [SerializeField] private Image chapterImage;
    [SerializeField] private TMP_Text chapterNameText;
    [SerializeField] private GameObject lockObject;
    [SerializeField] private Button button;

    private StageSelectManager stageSelectManager;
    private int chapterIndex;

    public void Initialize(
        StageSelectManager manager,
        int index,
        ChapterData chapter,
        bool unlocked,
        bool cleared)
    {
        stageSelectManager = manager;
        chapterIndex = index;

        chapterImage.sprite = chapter.ChapterImage;
        chapterNameText.text = chapter.ChapterName;
        lockObject.SetActive(!unlocked);
        button.interactable = unlocked;

        button.onClick.RemoveListener(OnClick);
        button.onClick.AddListener(OnClick);
    }

    private void OnClick()
    {
        if (stageSelectManager == null)
        {
            return;
        }

        Debug.Log($"ステージ{chapterIndex}");
        stageSelectManager.SelectChapter(chapterIndex);
    }
}