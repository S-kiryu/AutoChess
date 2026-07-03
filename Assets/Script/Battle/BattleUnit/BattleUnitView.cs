using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class BattleUnitView : MonoBehaviour
{
    [SerializeField] private Image unitImage;
    [SerializeField] private float damageFlashDuration = 0.15f;

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
        unitImage.color = defaultColor;
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
            unitImage.color = defaultColor;
        }
    }

    private IEnumerator DamageFlashRoutine()
    {
        unitImage.color = Color.red;

        yield return new WaitForSeconds(damageFlashDuration);

        unitImage.color = defaultColor;
        damageFlashCoroutine = null;
    }

    private void OnDisable()
    {
        ResetColor();
        damageFlashCoroutine = null;
    }
}