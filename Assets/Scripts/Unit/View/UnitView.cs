using TMPro;
using UnityEngine;

public class UnitView : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI hpText;

    public void SetHP(int hp)
    {
        hpText.text = hp.ToString();
    }

    public void SetPosition(Vector3 worldPos)
    {
        transform.position = worldPos;
    }
}