using UnityEngine;
using UnityEngine.UI;

public class FormationUnitLevelUpUI : MonoBehaviour
{
    public static FormationUnitLevelUpUI Instance { get; private set; }

    [SerializeField] private GameObject panel;
    [SerializeField] private Image unitIcon;
    [SerializeField] private Text levelText;

    private UnitInstance selectedUnit;

    private void Awake()
    {
        Instance = this;
        panel.SetActive(false);
    }

    public void Show(UnitInstance unit)
    {
        selectedUnit = unit;
        panel.SetActive(true);
        Refresh();
    }

    public void Hide()
    {
        selectedUnit = null;
        panel.SetActive(false);
    }

    public void OnClickLevelUp()
    {
        if (selectedUnit == null) return;

        int cost = GetLevelUpCost(selectedUnit);

        if (CoinManager.Instanse == null)
        {
            Debug.LogWarning("CoinManager がありません");
            return;
        }

        if (!selectedUnit.CanLevelUp)
        {
            Debug.Log("これ以上レベルを上げられません");
            return;
        }

        if (!CoinManager.Instanse.TryPay(cost))
        {
            return;
        }

        selectedUnit.LevelUp();
        Refresh();
    }

    private int GetLevelUpCost(UnitInstance unit)
    {
        return unit.Level switch
        {
            1 => 1,
            2 => 2,
            3 => 4,
            4 => 6,
            5 => 8,
            _ => unit.Level * 2
        };
    }

    private void Refresh()
    {
        if (selectedUnit == null) return;

        if (levelText != null)
        {
            levelText.text = $"Lv {selectedUnit.Status.Level}";
        }

        if (unitIcon != null)
        {
            unitIcon.sprite = selectedUnit.Data.Icon;
            unitIcon.enabled = selectedUnit.Data.Icon != null;
        }
    }
}