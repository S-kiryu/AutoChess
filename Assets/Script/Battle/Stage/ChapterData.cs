using UnityEngine;

[CreateAssetMenu(menuName = "AutoChess/ChapterData")]
public class ChapterData : ScriptableObject
{
    [SerializeField] private string chapterId;
    [SerializeField] private string chapterName;
    [SerializeField] private Sprite chapterImage;
    [SerializeField] private BattleStageData[] battleStages;

    public string ChapterId => chapterId;
    public string ChapterName => chapterName;
    public Sprite ChapterImage => chapterImage;
    public BattleStageData[] BattleStages => battleStages;
}