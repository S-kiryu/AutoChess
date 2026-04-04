using TMPro;
using UnityEngine;

public class UnitView : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI hpText;

    public void SetHP(int hp)
    {
        hpText.text = hp.ToString();
    }
}