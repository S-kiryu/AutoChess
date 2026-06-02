using UnityEngine;

public class GameLoopManager : MonoBehaviour
{
    public GameState CurrentState { get; private set; }

    [SerializeField] private GameState initialState;

    private void Start()
    {
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
        // 準備フェーズの処理
        Debug.Log("準備フェーズに入りました。");
    }

    private void OnBattle()
    {
        // 戦闘フェーズの処理
        Debug.Log("戦闘フェーズに入りました。");
    }

    private void OnReward()
    {
        // 報酬フェーズの処理
        Debug.Log("報酬フェーズに入りました。");
    }
}