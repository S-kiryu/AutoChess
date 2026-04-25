using TMPro;
using UnityEngine;

public class UnitView : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI hpText;

    public UnitModel Model { get; private set; }
    public TeamType Team => Model.Team;

    public void Bind(UnitModel model)
    {
        Model = model;
        SetHP(model.CurrentHp);
    }

    public void SetHP(int hp)
    {
        hpText.text = hp.ToString();
    }

    public void SetPosition(Vector3 worldPos)
    {
        transform.position = worldPos;
    }
}
