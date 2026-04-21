using UnityEngine;

public interface IGameState
{
    void Enter();
    void Update();
    void Exit();
}

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    private IGameState currentState;

    public void ChangeState(IGameState newState)
    {
        currentState?.Exit();   // 前の状態を終了

        currentState = newState;

        currentState.Enter();   // 新しい状態開始
    }

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Update()
    {
        currentState?.Update();
    }


    //#region //バトルシステム
    //[SerializeField] private BaseStatus playerData;

    //[SerializeField] private BaseStatus enemyData;



    //[SerializeField] private UnitView playerView;

    //[SerializeField] private UnitView enemyView;


    //private BattlePresenter presenter;

    //void Start()
    //{
    //    var player = new UnitModel(playerData, UnitModel.TeamType.Player);
    //    var enemy = new UnitModel(enemyData, UnitModel.TeamType.Enemy);

    //    presenter = new BattlePresenter(player, enemy, playerView, enemyView);
    //}

    //public void OnAttackButton()
    //{
    //    presenter.Attack();
    //}
    //#endregion

}

