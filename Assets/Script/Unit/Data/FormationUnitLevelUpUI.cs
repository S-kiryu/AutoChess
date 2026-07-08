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

        if (!CoinManager.Instanse.TryPay(cost))
        {
            Debug.Log($"コインが足りないためレベルアップできません。必要: {cost}");
            return;
        }

        selectedUnit.Status.LevelUp(1);
        Refresh();
    }

    private int GetLevelUpCost(UnitInstance unit)
    {
        return unit.Status.Level switch
        {
            1 => 1,
            2 => 2,
            3 => 4,
            4 => 6,
            5 => 8,
            _ => unit.Status.Level * 2
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