using UnityEngine;
using UnityEngine.UI;

public class ShopView : MonoBehaviour
{
    [SerializeField] private Image _unitIcons;

    // ユニットアイコンを更新するメソッド
    public void UpdateUnitIcons(UnitData units)
    {
        _unitIcons.sprite = units.Icon;
        _unitIcons.gameObject.SetActive(true);   
        Debug.Log("更新");
    }
}
