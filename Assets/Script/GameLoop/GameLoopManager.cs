using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameLoopManager : MonoBehaviour
{
    public GameState CurrentState { get; private set; }
    public static GameLoopManager Instance { get; private set; }

    [SerializeField] private GameState initialState;
    [SerializeField] private BattleUnitSpawner battleUnitSpawner;
    [SerializeField] private StageProgressManager stageProgressManager;
    [SerializeField] private GameObject nextButton;
    [SerializeField] private string homeSceneName = "Home";

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    private IEnumerator Start()
    {
        while (BattleGridManager.Instance == null ||
               !BattleGridManager.Instance.IsReady)
        {
            yield return null;
        }

        ChangeState(initialState);
    }

    public void ChangeState(GameState newState)
    {
        CurrentState = newState;

        switch (newState)
        {
            case GameState.Preparation:
                OnPreparation();
                break;
            case GameState.Battle:
                OnBattle();
                break;
            case GameState.Reward:
                OnReward();
                break;
        }
    }

    private void OnPreparation()
    {
        nextButton.SetActive(false);

        if (battleUnitSpawner != null)
        {
            battleUnitSpawner.SpawnEnemies(stageProgressManager.CurrentBattleStage);
        }

        Debug.Log("準備フェーズに入りました。");
    }

    private void OnBattle()
    {
        Debug.Log("戦闘フェーズに入りました。");

        if (BattleManager.Instance != null)
        {
            BattleManager.Instance.StartBattle();
        }
    }

    private void OnReward()
    {
        nextButton.SetActive(true);
        Debug.Log("報酬フェーズに入りました。");
    }

    public void OnRewardNextButton()
    {
        if (stageProgressManager != null &&
            stageProgressManager.ChapterClearPending)
        {
            stageProgressManager.ClearChapterClearPending();
            SceneManager.LoadScene(homeSceneName);
            return;
        }

        if (stageProgressManager != null)
        {
            stageProgressManager.NextBattleStage();
        }

        ChangeState(GameState.Preparation);
    }
}