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
    [SerializeField] private Animator animator;
    [SerializeField] private UnitImageAnimator unitImageAnimator;

    private Color currentBaseColor = Color.white;

    private Coroutine damageFlashCoroutine;
    private UnitInstance currentUnit;
    private Color defaultColor = Color.white;
    private static readonly int IsMovingHash = Animator.StringToHash("IsMoving");
    private static readonly int DirectionHash = Animator.StringToHash("Direction");
    private static readonly int AttackHash = Animator.StringToHash("Attack");

    private void Awake()
    {
        if (unitImage == null)
        {
            unitImage = GetComponentInChildren<Image>();
        }

        if (animator == null)
        {
            animator = GetComponentInChildren<Animator>();
        }

        if (unitImage != null)
        {
            defaultColor = unitImage.color;
            currentBaseColor = defaultColor;
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

        if (unitImageAnimator != null)
        {
            unitImageAnimator.SetAnimationData(unit.Data.AnimationData);
        }

        currentUnit = unit;
        currentUnit.OnStarChanged += ApplyStarColor;

        currentBaseColor = unitImage.color;
    }
    public void PlayMove(Vector2Int direction)
    {
        SetDirection(direction);

        if (animator != null)
        {
            animator.SetBool(IsMovingHash, true);
        }
    }

    public void StopMove()
    {
        if (animator != null)
        {
            animator.SetBool(IsMovingHash, false);
        }
    }

    public void PlayAttack(Vector2Int direction)
    {
        SetDirection(direction);

        if (animator != null)
        {
            animator.SetBool(IsMovingHash, false);
            animator.SetTrigger(AttackHash);
        }
    }


    private void ApplyStarColor(int star)
    {
        currentBaseColor = GetStarColor(star);
        unitImage.color = currentBaseColor;
    }

    private void OnDestroy()
    {
        if (currentUnit != null)
        {
            currentUnit.OnStarChanged -= ApplyStarColor;
        }
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
        if (damageFlashCoroutine != null)
        {
            StopCoroutine(damageFlashCoroutine);
            damageFlashCoroutine = null;
        }

        ResetColor();
    }

    private void SetDirection(Vector2Int direction)
    {
        if (animator == null)
        {
            return;
        }

        animator.SetInteger(DirectionHash, DirectionToIndex(direction));
    }

    private int DirectionToIndex(Vector2Int direction)
    {
        if (direction == Vector2Int.up) return 1;
        if (direction == Vector2Int.down) return 0;
        if (direction == Vector2Int.left) return 2;
        if (direction == Vector2Int.right) return 3;

        return 1;
    }
}