using UnityEngine;
using UnityEngine.UI;

public class FormationGrid : MonoBehaviour
{
    [SerializeField] private Image icon;

    public void SetData(UnitData data)
    {
        if (data == null)
        {
            icon.enabled = false;
        }
        else
        {
            icon.enabled = true;
            icon.sprite = data.Icon;
        }
    }
}