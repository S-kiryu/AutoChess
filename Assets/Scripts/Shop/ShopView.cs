using UnityEngine;
using UnityEngine.UI;

public class ShopView : MonoBehaviour
{
    [SerializeField] private Image[] _unitIcons;

    // ユニットアイコンを更新するメソッド
    public void UpdateUnitIcons(UnitData[] units)
    {
        for (int i = 0; i < _unitIcons.Length; i++)
        {
            if (i < units.Length)
            {
                _unitIcons[i].sprite = units[i].Icon;
                _unitIcons[i].gameObject.SetActive(true);
            }
            else
            {
                _unitIcons[i].gameObject.SetActive(false);
            }
        }
        Debug.Log("更新");
    }
}
