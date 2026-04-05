using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private BaseStatus playerData;

    [SerializeField] private BaseStatus enemyData;



    [SerializeField] private UnitView playerView;
    [SerializeField] private UnitView enemyView;

    private BattlePresenter presenter;

    void Start()
    {
        var player = new UnitModel(playerData);
        var enemy = new UnitModel(enemyData);

        presenter = new BattlePresenter(player, enemy, playerView, enemyView);
    }

    public void OnAttackButton()
    {
        presenter.Attack();
    }
}