using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 個々のスロットのUIを管理するクラス
/// </summary>
public class BenchSlotUI : MonoBehaviour
{
    [SerializeField] private Image _unitIcon;
    [SerializeField] private GameObject _highlight;

    /// <summary>
    /// ユニットを配置する
    /// </summary>
    /// <param name="unit"></param>
    public void SetUnit(UnitInstance unit)
    {
        _unitIcon.sprite = unit.Data.Icon;
        _unitIcon.enabled = true;
    }

    /// <summary>
    /// ユニットを撤去する
    /// </summary>
    public void Clear()
    {
        _unitIcon.sprite = null;
        _unitIcon.enabled = false;
    }

    /// <summary>
    /// ユニットが選択されているかどうかを表示するハイライト
    /// </summary>
    /// <param name="selected"></param>
    public void SetSelected(bool selected)
    {
        _highlight.SetActive(selected);
    }

    /// <summary>
    /// 置いた時の演出
    /// </summary>
    public void PlayPlaceEffect()
    {
        // 光る演出、拡大縮小、パーティクルなど
    }

    /// <summary>
    /// 消した時の演出
    /// </summary>
    public void PlayRemoveEffect()
    {
        // 消える演出、縮小、パーティクルなど
    }
}