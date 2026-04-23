using UnityEngine;

public class BattleTestStarter : MonoBehaviour
{
    [SerializeField] private UnitPresenter unitPresenter;
    [SerializeField] private BaseStatus allyStatus;
    [SerializeField] private BaseStatus enemyStatus;
    [SerializeField] private UnitView allyView;
    [SerializeField] private UnitView enemyView;

    private void Start()
    {
        UnitModel ally = new UnitModel(allyStatus, UnitModel.TeamType.Player);
        UnitModel enemy = new UnitModel(enemyStatus, UnitModel.TeamType.Enemy);

        unitPresenter.RegisterUnit(ally, allyView, new Vector2Int(0, 0));
        unitPresenter.RegisterUnit(enemy, enemyView, new Vector2Int(5,9));
    }
}
