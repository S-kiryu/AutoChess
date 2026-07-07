using System.Collections;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 戦闘時のユニットの見た目を管理するクラス
/// </summary>
public class BattleUnitView : MonoBehaviour
{
    [SerializeField] private Image unitImage;
    [SerializeField] private float damageFlashDuration = 0.15f;
    [SerializeField] private Color star1Color = Color.white;
    [SerializeField] private Color star2Color = new Color(0.4f, 0.8f, 1f);
    [SerializeField] private Color star3Color = new Color(1f, 0.75f, 0.2f);

    private Color currentBaseColor = Color.white;

    private Coroutine damageFlashCoroutine;
    private Color defaultColor = Color.white;

    private void Awake()
    {
        if (unitImage == null)
        {
            unitImage = GetComponentInChildren<Image>();
        }

        if (unitImage != null)
        {
            defaultColor = unitImage.color;
        }
    }

    public void SetUnit(UnitInstance unit)
    {
        if (unitImage == null || unit == null || unit.Data == null)
        {
            return;
        }

        unitImage.sprite = unit.Data.Icon;
        unitImage.enabled = true;

        currentBaseColor = GetStarColor(unit.Star);
        unitImage.color = currentBaseColor;
    }

    private Color GetStarColor(int star)
    {
        return star switch
        {
            1 => star1Color,
            2 => star2Color,
            3 => star3Color,
            _ => star3Color
        };
    }

    public void PlayDamageFlash()
    {
        if (unitImage == null)
        {
            return;
        }

        if (damageFlashCoroutine != null)
        {
            StopCoroutine(damageFlashCoroutine);
        }

        damageFlashCoroutine = StartCoroutine(DamageFlashRoutine());
    }

    public void ResetColor()
    {
        if (unitImage != null)
        {
            unitImage.color = currentBaseColor;
        }
    }

    /// <summary>
    /// ダメージを受けたときのフラッシュエフェクトを再生するコルーチン
    /// </summary>
    /// <returns></returns>
    private IEnumerator DamageFlashRoutine()
    {
        unitImage.color = Color.red;

        yield return new WaitForSeconds(damageFlashDuration);

        unitImage.color = currentBaseColor;
        damageFlashCoroutine = null;
    }

    private void OnDisable()
    {
        ResetColor();
        damageFlashCoroutine = null;
    }
}