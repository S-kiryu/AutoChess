using UnityEngine;

public class Unit : MonoBehaviour
{
    [SerializeField] private TeamType _teamType;
    [SerializeField] private BaseStatus _baseStatus;
    [SerializeField] private UnitView _unitView;

    public UnitModel UnitModel { get; private set; }

    public TeamType TeamType => _teamType;
    public BaseStatus BaseStatus => _baseStatus;
    public UnitView UnitView => _unitView;

    private void Awake()
    {
        UnitModel = new UnitModel(_baseStatus, _teamType);
    }
}
