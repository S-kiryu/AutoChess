using UnityEngine;

/// <summary>
/// チャプターを保存するやつ
/// </summary>
[CreateAssetMenu(menuName = "AutoChess/ChapterData")]
public class ChapterData : ScriptableObject
{
    [SerializeField] private string chapterName;
    [SerializeField] private BattleStageData[] battleStages;

    public string ChapterName => chapterName;
    public BattleStageData[] BattleStages => battleStages;
}