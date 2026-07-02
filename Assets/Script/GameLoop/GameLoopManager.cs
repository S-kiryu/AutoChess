using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GameLoopManager : MonoBehaviour
{
    public GameState CurrentState { get; private set; }
    public static GameLoopManager Instance { get; private set; }

    [SerializeField] private GameState initialState;
    [SerializeField] private EnemySpawner enemySpawner;
    [SerializeField] private StageProgressManager stageProgressManager;
    [SerializeField] private GameObject nextButton;

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
        // 準備フェーズの処理
        enemySpawner.SpawnEnemies(stageProgressManager.CurrentBattleStage);
        Debug.Log("準備フェーズに入りました。");
    }

    private void OnBattle()
    {
        // 戦闘フェーズの処理
        Debug.Log("戦闘フェーズに入りました。");
        if (BattleManager.Instance != null)
        {
            BattleManager.Instance.StartBattle();
        }
    }

    private void OnReward()
    {
        nextButton.SetActive(true);
        // 報酬フェーズの処理
        Debug.Log("報酬フェーズに入りました。");
    }
}